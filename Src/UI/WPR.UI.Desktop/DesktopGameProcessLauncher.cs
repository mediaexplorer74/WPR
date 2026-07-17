using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WPR.Common;
using WPR.Models;

namespace WPR.UI.Desktop
{
    internal static class DesktopGameProcessLauncher
    {
        private const string DiagnosticFolder = "Logs";

        public static void RegisterUnhandledExceptionDiagnostics(string productId)
        {
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                try
                {
                    string diagnosticPath = GetDiagnosticPath(productId);
                    Directory.CreateDirectory(Path.GetDirectoryName(diagnosticPath)!);
                    File.WriteAllText(diagnosticPath,
                        $"Unhandled game-host exception at {DateTime.UtcNow:o}\n{args.ExceptionObject}");
                }
                catch
                {
                    // The process is terminating; there is no safe recovery path here.
                }
            };
        }

        public static async Task Start(Application app)
        {
            string productId = NormalizeProductId(app.ProductId);
            string diagnosticPath = GetDiagnosticPath(productId);

            if (File.Exists(diagnosticPath))
            {
                File.Delete(diagnosticPath);
            }

            using Process process = Process.Start(CreateStartInfo(productId))
                ?? throw new InvalidOperationException("The WPR game host process could not be started.");
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                return;
            }

            string details = File.Exists(diagnosticPath)
                ? await File.ReadAllTextAsync(diagnosticPath)
                : "The game host terminated without managed diagnostics.";
            throw new InvalidOperationException(
                $"The isolated game host exited with code {process.ExitCode}.\n{details}");
        }

        public static async Task<int> RunGameHost(
            IClassicDesktopStyleApplicationLifetime lifetime, string productId)
        {
            string diagnosticPath = GetDiagnosticPath(productId);

            try
            {
                MessageBoxUtils.MainWindow = lifetime.MainWindow
                    ?? throw new InvalidOperationException("The game host window was not initialized.");
                ServicesSetup.Start();

                Application app = ApplicationContext.Current.Applications!
                    .SingleOrDefault(item => item.ProductId.ToLower() == productId)
                    ?? throw new InvalidOperationException(
                        $"No installed application has ProductID '{productId}'.");

                await ApplicationLaunch.Start(app, default);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppList, $"Isolated game host failed:\n{ex}");

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(diagnosticPath)!);
                    await File.WriteAllTextAsync(diagnosticPath, ex.ToString());
                }
                catch (Exception diagnosticException)
                {
                    Log.Warn(LogCategory.AppList,
                        $"Failed to write isolated game host diagnostics:\n{diagnosticException}");
                }

                return 1;
            }
        }

        private static ProcessStartInfo CreateStartInfo(string productId)
        {
            string processPath = Environment.ProcessPath
                ?? throw new InvalidOperationException("The current WPR executable path is unavailable.");
            var startInfo = new ProcessStartInfo(processPath)
            {
                UseShellExecute = false,
                WorkingDirectory = AppContext.BaseDirectory
            };

            if (Path.GetFileNameWithoutExtension(processPath)
                .Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            {
                string entryAssemblyPath = Assembly.GetEntryAssembly()?.Location ?? "";
                if (string.IsNullOrWhiteSpace(entryAssemblyPath))
                {
                    throw new InvalidOperationException("The WPR entry assembly path is unavailable.");
                }

                startInfo.ArgumentList.Add(entryAssemblyPath);
            }

            startInfo.ArgumentList.Add(Program.GameHostArgument);
            startInfo.ArgumentList.Add(productId);
            return startInfo;
        }

        private static string GetDiagnosticPath(string productId)
        {
            return Configuration.Current!.DataPath(
                Path.Combine(DiagnosticFolder, $"game-host-{productId}.txt"));
        }

        private static string NormalizeProductId(string? productId)
        {
            if (!Guid.TryParse(productId, out Guid parsedProductId))
            {
                throw new InvalidDataException("The installed application's ProductID is not a valid GUID.");
            }

            return parsedProductId.ToString("D");
        }
    }
}
