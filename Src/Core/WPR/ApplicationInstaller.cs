using Mono.Cecil;
using System;
using System.IO.Compression;
using System.Reflection;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using WPR.Common;
using WPR.Models;
using WPR.Core;

namespace WPR
{
    public static class ApplicationInstaller
    {
        private const int MaxArchiveEntries = 100_000;
        private const long MaxArchiveUncompressedBytes = 16L * 1024 * 1024 * 1024;

        private static XmlDocument LoadXmlDocument(ZipArchiveEntry entry)
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using Stream stream = entry.Open();
            using XmlReader reader = XmlReader.Create(stream, settings);
            var document = new XmlDocument
            {
                XmlResolver = null
            };
            document.Load(reader);
            return document;
        }

        private static string ValidateProductId(string productId)
        {
            if (!Guid.TryParse(productId, out Guid parsedProductId))
            {
                throw new InvalidDataException("The application ProductID is not a valid GUID.");
            }

            return parsedProductId.ToString("D");
        }

        private static string GetSafeExtractionPath(string extractionRoot, string entryName)
        {
            if (string.IsNullOrWhiteSpace(entryName) || Path.IsPathFullyQualified(entryName))
            {
                throw new InvalidDataException($"Archive entry has an invalid path: '{entryName}'.");
            }

            string[] pathSegments = entryName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (pathSegments.Length == 0 || pathSegments.Any(segment =>
                    segment == "." || segment == ".." ||
                    segment.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                    segment.Contains(':')))
            {
                throw new InvalidDataException($"Archive entry has an unsafe path: '{entryName}'.");
            }

            string root = Path.GetFullPath(extractionRoot)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string normalizedEntryName = string.Join(Path.DirectorySeparatorChar, pathSegments);
            string destination = Path.GetFullPath(Path.Combine(root, normalizedEntryName));
            StringComparison comparison = OperatingSystem.IsWindows()
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            if (!destination.StartsWith(root, comparison))
            {
                throw new InvalidDataException($"Archive entry escapes the application directory: '{entryName}'.");
            }

            return destination;
        }

        private static void ValidateArchive(ZipArchive archive, string extractionRoot)
        {
            if (archive.Entries.Count > MaxArchiveEntries)
            {
                throw new InvalidDataException($"Archive contains more than {MaxArchiveEntries} entries.");
            }

            long totalUncompressedBytes = 0;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                totalUncompressedBytes = checked(totalUncompressedBytes + entry.Length);
                if (totalUncompressedBytes > MaxArchiveUncompressedBytes)
                {
                    throw new InvalidDataException("Archive expands beyond the supported size limit.");
                }

                GetSafeExtractionPath(extractionRoot, entry.FullName);
            }
        }

        private static async Task<(ApplicationInstallError Error, Application? App, string StagingFolder, string FinalFolder, List<Application> ExistingApps)> CreateApplicationEntryAndExtract(Stream fileStream, Action<int> progressSet, Func<Application, IObservable<bool>> deleteExistingApp, CancellationToken canceled)
        {
            Application? app = null;
            string stagingFolder = "";
            string finalFolder = "";
            List<Application> existingApps = new();

            try
            {
                using ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read, leaveOpen: true);
                ZipArchiveEntry? entry = archive.GetEntry("WMAppManifest.xml");
                if (entry == null)
                {
                    return (ApplicationInstallError.MissingManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                if (canceled.IsCancellationRequested)
                {
                    return (ApplicationInstallError.Canceled, app, stagingFolder, finalFolder, existingApps);
                }

                progressSet(3);

                XmlDocument wmManifestDoc = LoadXmlDocument(entry);

                XmlElement? root = wmManifestDoc.DocumentElement;
                XmlNodeList? appNodeList = root!.SelectNodes("//App");

                if (appNodeList == null || appNodeList.Count == 0)
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                XmlNode? appNode = appNodeList[0];

                XmlAttribute? titleAttrib = appNode!.Attributes!["Title"];
                XmlAttribute? runtimeTypeAttrb = appNode!.Attributes!["RuntimeType"];
                XmlAttribute? versionAttrib = appNode!.Attributes!["Version"];
                XmlAttribute? productAttrib = appNode!.Attributes!["ProductID"];

                if ((titleAttrib == null) || (runtimeTypeAttrb == null) || (versionAttrib == null) || (productAttrib == null))
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                string productTrimmed = ValidateProductId(productAttrib!.Value);
                string storeFolder = Configuration.Current!.DataPath(Application.DataStoreFolder);
                string productStoreFolder = Path.Combine(storeFolder, productTrimmed);
                string productStoreFolderRelative = Path.Combine(Application.DataStoreFolder, productTrimmed);

                if (!Enum.TryParse(runtimeTypeAttrb.Value, true, out ApplicationType runtimeTypeParsed))
                {
                    return (ApplicationInstallError.NotSupportedAppType, app, stagingFolder, finalFolder, existingApps);
                }

                XmlAttribute? authorAttrib = appNode!.Attributes!["Author"];
                XmlAttribute? publisherAttrib = appNode!.Attributes!["Publisher"];
                XmlAttribute? descriptionAttrib = appNode!.Attributes!["Description"];

                XmlNodeList? iconPathNodes = appNode!.SelectNodes("//IconPath");
                String? iconPath = null;

                if (iconPathNodes != null && iconPathNodes.Count > 0)
                {
                    iconPath = iconPathNodes[0]!.InnerText;
                }

                entry = archive.GetEntry("AppManifest.xaml");
                if (entry == null)
                {
                    return (ApplicationInstallError.MissingManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                if (canceled.IsCancellationRequested)
                {
                    return (ApplicationInstallError.Canceled, app, stagingFolder, finalFolder, existingApps);
                }

                wmManifestDoc = LoadXmlDocument(entry);

                XmlNode? deploymentNode = wmManifestDoc.DocumentElement;

                XmlAttribute? entryPointAsmAttrib = deploymentNode!.Attributes!["EntryPointAssembly"];
                XmlAttribute? entryPointTypeAttrib = deploymentNode!.Attributes!["EntryPointType"];
                
                if ((entryPointAsmAttrib == null) || (entryPointTypeAttrib == null))
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                var nsmgr = new XmlNamespaceManager(wmManifestDoc.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.microsoft.com/client/2007/deployment");

                XmlNodeList? assemblies = deploymentNode!.SelectNodes("//a:Deployment.Parts//a:AssemblyPart", nsmgr);
                if (assemblies == null)
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                XmlAttribute? entryPointAsmFileNameAttrib = null;

                foreach (XmlNode? assembly in assemblies)
                {
                    XmlAttribute? attrib = assembly!.Attributes!["x:Name"] ?? assembly!.Attributes!["Name"];
                    if ((attrib == null) || (attrib.Value != entryPointAsmAttrib.Value))
                    {
                        continue;
                    }
                    entryPointAsmFileNameAttrib = assembly!.Attributes!["Source"];
                }

                if (entryPointAsmFileNameAttrib == null)
                {
                    return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
                }

                existingApps = await ApplicationContext.Current.Applications!
                    .Where(a => a.ProductId.ToLower() == productTrimmed)
                    .ToListAsync(canceled);

                if (existingApps.Count != 0 && !await deleteExistingApp(existingApps[0]))
                {
                    return (ApplicationInstallError.Canceled, app, stagingFolder, finalFolder, existingApps);
                }

                progressSet(5);

                Directory.CreateDirectory(storeFolder);
                finalFolder = productStoreFolder;
                stagingFolder = Path.Combine(storeFolder, $".install-{productTrimmed}-{Guid.NewGuid():N}");
                ValidateArchive(archive, stagingFolder);

                Directory.CreateDirectory(stagingFolder);
                int count = 0;

                foreach (ZipArchiveEntry iterateEntry in archive.Entries)
                {
                    if (canceled.IsCancellationRequested)
                    {
                        return (ApplicationInstallError.Canceled, app, stagingFolder, finalFolder, existingApps);
                    }
                    string fileDestinationPath = GetSafeExtractionPath(stagingFolder, iterateEntry.FullName);

                    if (iterateEntry.Name.Length == 0)
                    {
                        Directory.CreateDirectory(fileDestinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath)!);
                        iterateEntry.ExtractToFile(fileDestinationPath, true);
                    }

                    count++;
                    progressSet(5 + (int)((float)count / archive.Entries.Count * 50.0f));
                }

                app = new Application()
                {
                    Name = titleAttrib.Value,
                    ApplicationType = runtimeTypeParsed,
                    Version = versionAttrib.Value,
                    IconPath = (iconPath == null) ? "" : Path.Combine(productStoreFolderRelative, iconPath),
                    Publisher = (publisherAttrib == null) ? "Unknown" : publisherAttrib.Value,
                    Author = (authorAttrib == null) ? "Unknown" : authorAttrib.Value,
                    Description = (descriptionAttrib == null) ? "" : descriptionAttrib.Value,
                    ProductId = productTrimmed,
                    Assembly = entryPointAsmFileNameAttrib.Value,
                    EntryPoint = entryPointTypeAttrib.Value,
                    InstalledTime = DateTime.Now,
                    PatchedVersion = 0
                };

                progressSet(60);
            }
            catch (OperationCanceledException) when (canceled.IsCancellationRequested)
            {
                return (ApplicationInstallError.Canceled, app, stagingFolder, finalFolder, existingApps);
            }
            catch (InvalidDataException ex)
            {
                Log.Error(LogCategory.AppInstall, $"Application archive is invalid: \n{ex}");
                return (ApplicationInstallError.InvalidManifestFiles, app, stagingFolder, finalFolder, existingApps);
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppInstall, $"An unexpected error happen during the installation: \n{ex}");
                return (ApplicationInstallError.UnexpectedError, app, stagingFolder, finalFolder, existingApps);
            }

            return (ApplicationInstallError.None, app, stagingFolder, finalFolder, existingApps);
        }

        private static async Task CommitInstallation(Application app, IReadOnlyCollection<Application> existingApps,
            string stagingFolder, string finalFolder, CancellationToken cancellationToken)
        {
            string backupFolder = $"{finalFolder}.backup-{Guid.NewGuid():N}";
            bool hasBackup = false;

            try
            {
                if (Directory.Exists(finalFolder))
                {
                    Directory.Move(finalFolder, backupFolder);
                    hasBackup = true;
                }

                Directory.Move(stagingFolder, finalFolder);

                await using var transaction = await ApplicationContext.Current.Database
                    .BeginTransactionAsync(cancellationToken);
                ApplicationContext.Current.Applications!.RemoveRange(existingApps);
                ApplicationContext.Current.Applications!.Add(app);
                await ApplicationContext.Current.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                try
                {
                    if (Directory.Exists(finalFolder))
                    {
                        Directory.Delete(finalFolder, true);
                    }

                    if (hasBackup && Directory.Exists(backupFolder))
                    {
                        Directory.Move(backupFolder, finalFolder);
                    }
                }
                finally
                {
                    ApplicationContext.Current.ChangeTracker.Clear();
                }

                throw;
            }

            if (hasBackup && Directory.Exists(backupFolder))
            {
                try
                {
                    Directory.Delete(backupFolder, true);
                }
                catch (Exception ex)
                {
                    Log.Warn(LogCategory.AppInstall,
                        $"The previous application files could not be removed from '{backupFolder}':\n{ex}");
                }
            }
        }

        public static async Task Uninstall(Application app, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(app.ProductId, out Guid productId))
            {
                throw new InvalidDataException("The installed application's ProductID is not a valid GUID.");
            }

            string appStoreFolder = Configuration.Current!.DataPath(Application.DataStoreFolder);
            var movedDirectories = new List<(string Original, string Quarantine)>();

            try
            {
                if (Directory.Exists(appStoreFolder))
                {
                    foreach (string directory in Directory.EnumerateDirectories(appStoreFolder))
                    {
                        string directoryName = Path.GetFileName(directory);
                        if (!Guid.TryParse(directoryName, out Guid directoryProductId) ||
                            directoryProductId != productId)
                        {
                            continue;
                        }

                        cancellationToken.ThrowIfCancellationRequested();
                        string quarantine = Path.Combine(appStoreFolder,
                            $".uninstall-{productId:D}-{Guid.NewGuid():N}");
                        Directory.Move(directory, quarantine);
                        movedDirectories.Add((directory, quarantine));
                    }
                }

                await using var transaction = await ApplicationContext.Current.Database
                    .BeginTransactionAsync(cancellationToken);
                List<Application> databaseEntries = await ApplicationContext.Current.Applications!
                    .Where(item => item.Id == app.Id || item.ProductId.ToLower() == productId.ToString("D"))
                    .ToListAsync(cancellationToken);
                ApplicationContext.Current.Applications!.RemoveRange(databaseEntries);
                await ApplicationContext.Current.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                try
                {
                    foreach ((string original, string quarantine) in movedDirectories.AsEnumerable().Reverse())
                    {
                        if (Directory.Exists(quarantine) && !Directory.Exists(original))
                        {
                            Directory.Move(quarantine, original);
                        }
                    }
                }
                finally
                {
                    ApplicationContext.Current.ChangeTracker.Clear();
                }

                throw;
            }

            foreach ((string _, string quarantine) in movedDirectories)
            {
                try
                {
                    Directory.Delete(quarantine, true);
                }
                catch (Exception ex)
                {
                    Log.Warn(LogCategory.AppInstall,
                        $"Uninstalled application files could not be deleted from '{quarantine}':\n{ex}");
                }
            }
        }

        public static async Task<ApplicationInstallError> Install(Stream fileStream, Action<int> progressSet, Func<Application, IObservable<bool>> deleteExistingApp, CancellationToken cancelSource)
        {
            string stagingFolder = "";

            try
            {
                Application? app;
                string finalFolder;
                List<Application> existingApps;
                ApplicationInstallError error;

                // 60% spend for extracting files
                (error, app, stagingFolder, finalFolder, existingApps) = await Task.Run(() =>
                    CreateApplicationEntryAndExtract(fileStream, progressSet, deleteExistingApp, cancelSource));

                if (error != ApplicationInstallError.None)
                {
                    return error;
                }

                // 20% for patching DLLs
                try
                {
                    await Task.Run(() =>
                    {
                        ApplicationPatcher patcher = new ApplicationPatcher();
                        patcher.Patch(stagingFolder, progress => progressSet(60 + (int)((double)progress / 5)), cancelSource);
                    });
                }
                catch (OperationCanceledException) when (cancelSource.IsCancellationRequested)
                {
                    return ApplicationInstallError.Canceled;
                }
                catch (Exception exception)
                {
                    Log.Error(LogCategory.AppInstall, $"Application DLL patching failed with exception:\n{exception}");
                    return ApplicationInstallError.PatchFailed;
                }

                if (cancelSource.IsCancellationRequested)
                {
                    return ApplicationInstallError.Canceled;
                }

                app!.PatchedVersion = ApplicationPatcher.Version;

                // 20% for converting audio to unified support
                if (app.ApplicationType == ApplicationType.XNA)
                {
                    try
                    {
                        await AudioCompabilityConverter.ScanWmaAndConvert(stagingFolder,
                            progress => progressSet(80 + (int)((double)progress / 5)),
                            cancelSource);
                    }
                    catch (OperationCanceledException) when (cancelSource.IsCancellationRequested)
                    {
                        return ApplicationInstallError.Canceled;
                    }
                    catch (Exception exception)
                    {
                        Log.Error(LogCategory.AppInstall, $"Application WMA conversion failed with exception:\n{exception}");
                        return ApplicationInstallError.ConvertFailed;
                    }
                }

                if (cancelSource.IsCancellationRequested)
                {
                    return ApplicationInstallError.Canceled;
                }

                await CommitInstallation(app, existingApps, stagingFolder, finalFolder, cancelSource);

                progressSet(100);
            }
            catch (OperationCanceledException) when (cancelSource.IsCancellationRequested)
            {
                return ApplicationInstallError.Canceled;
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppInstall, $"An unexpected error happen during the installation: \n{ex}");
                return ApplicationInstallError.UnexpectedError;
            }
            finally
            {
                if (Directory.Exists(stagingFolder))
                {
                    try
                    {
                        Directory.Delete(stagingFolder, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(LogCategory.AppInstall,
                            $"The incomplete application staging directory could not be removed from '{stagingFolder}':\n{ex}");
                    }
                }
            }

            return ApplicationInstallError.None;
        }
    }
}
