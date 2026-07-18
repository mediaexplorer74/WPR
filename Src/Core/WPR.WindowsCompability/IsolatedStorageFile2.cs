using System;
using System.IO;
using System.Linq;

namespace WPR.WindowsCompability;

/// <summary>
/// Windows Phone-compatible isolated file store rooted in the current product's data directory.
/// </summary>
public sealed class IsolatedStorageFile2 : IDisposable
{
    private readonly string _rootPath;

    private IsolatedStorageFile2(string rootPath)
    {
        _rootPath = Path.GetFullPath(rootPath);
    }

    public static IsolatedStorageFile2 GetUserStoreForApplication()
    {
        string rootPath = IsolatedStorageSettingsSerializer.GetApplicationStoragePath()
            ?? throw new InvalidOperationException(
                "The application product ID and data-store configuration must be set before using isolated storage.");
        return new IsolatedStorageFile2(rootPath);
    }

    public long AvailableFreeSpace =>
        new DriveInfo(Path.GetPathRoot(_rootPath)!).AvailableFreeSpace;

    public long Quota => AvailableFreeSpace;

    public bool DirectoryExists(string path) => Directory.Exists(ResolvePath(path));

    public bool FileExists(string path) => File.Exists(ResolvePath(path));

    public void CreateDirectory(string dir) => Directory.CreateDirectory(ResolvePath(dir));

    public void DeleteDirectory(string dir) => Directory.Delete(ResolvePath(dir));

    public void DeleteFile(string file) => File.Delete(ResolvePath(file));

    public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite) =>
        File.Copy(ResolvePath(sourceFileName), ResolvePath(destinationFileName), overwrite);

    public void MoveFile(string sourceFileName, string destinationFileName) =>
        File.Move(ResolvePath(sourceFileName), ResolvePath(destinationFileName));

    public DateTimeOffset GetLastWriteTime(string path) =>
        new(File.GetLastWriteTime(ResolvePath(path)));

    public bool IncreaseQuotaTo(long newQuotaSize) => newQuotaSize >= 0;

    public void Remove()
    {
        Directory.Delete(_rootPath, recursive: true);
        Directory.CreateDirectory(_rootPath);
    }

    public IsolatedStorageFileStream2 CreateFile(string path) =>
        new(ResolvePath(path), FileMode.Create, FileAccess.ReadWrite, FileShare.None);

    public IsolatedStorageFileStream2 OpenFile(string path, FileMode mode) =>
        new(ResolvePath(path), mode, DefaultAccess(mode), FileShare.None);

    public IsolatedStorageFileStream2 OpenFile(string path, FileMode mode, FileAccess access) =>
        new(ResolvePath(path), mode, access, FileShare.None);

    public IsolatedStorageFileStream2 OpenFile(string path, FileMode mode, FileAccess access,
        FileShare share) => new(ResolvePath(path), mode, access, share);

    public string[] GetFileNames() => GetFileNames("*");

    public string[] GetFileNames(string searchPattern) =>
        EnumerateNames(searchPattern, Directory.GetFiles);

    public string[] GetDirectoryNames() => GetDirectoryNames("*");

    public string[] GetDirectoryNames(string searchPattern) =>
        EnumerateNames(searchPattern, Directory.GetDirectories);

    public void Dispose()
    {
    }

    private static FileAccess DefaultAccess(FileMode mode) =>
        mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite;

    internal string ResolvePath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        string relativePath = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);
        string resolvedPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        string rootPrefix = _rootPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        StringComparison comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!resolvedPath.Equals(_rootPath, comparison) &&
            !resolvedPath.StartsWith(rootPrefix, comparison))
        {
            throw new ArgumentException("The isolated-storage path escapes the application store.", nameof(path));
        }

        return resolvedPath;
    }

    private string[] EnumerateNames(string searchPattern,
        Func<string, string, string[]> enumerate)
    {
        ArgumentNullException.ThrowIfNull(searchPattern);
        string normalizedPattern = searchPattern
            .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
        string? directoryName = Path.GetDirectoryName(normalizedPattern);
        string pattern = Path.GetFileName(normalizedPattern);
        string directory = ResolvePath(directoryName ?? string.Empty);
        if (!Directory.Exists(directory))
        {
            return [];
        }

        return enumerate(directory, string.IsNullOrEmpty(pattern) ? "*" : pattern)
            .Select(Path.GetFileName)
            .Where(name => name != null)
            .Cast<string>()
            .ToArray();
    }
}

/// <summary>
/// File stream returned by <see cref="IsolatedStorageFile2"/>.
/// </summary>
public sealed class IsolatedStorageFileStream2 : FileStream
{
    internal IsolatedStorageFileStream2(string path, FileMode mode, FileAccess access,
        FileShare share) : base(path, mode, access, share)
    {
    }

    public IsolatedStorageFileStream2(string path, FileMode mode, IsolatedStorageFile2 store)
        : this(store.ResolvePath(path), mode,
            mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None)
    {
    }

    public IsolatedStorageFileStream2(string path, FileMode mode, FileAccess access,
        IsolatedStorageFile2 store) : this(store.ResolvePath(path), mode, access, FileShare.None)
    {
    }

    public IsolatedStorageFileStream2(string path, FileMode mode, FileAccess access,
        FileShare share, IsolatedStorageFile2 store)
        : this(store.ResolvePath(path), mode, access, share)
    {
    }

    public IsolatedStorageFileStream2(string path, FileMode mode, FileAccess access,
        FileShare share, int bufferSize, IsolatedStorageFile2 store)
        : base(store.ResolvePath(path), mode, access, share, bufferSize)
    {
    }
}
