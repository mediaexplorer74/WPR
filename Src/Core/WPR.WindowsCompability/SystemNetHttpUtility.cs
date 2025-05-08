using System.IO;

namespace WPR.WindowsCompability
{
    // Xamarin has failure of dealing with just a backslash
    public static class WebServices
    {
        public static string? Send(string? path)
        {
            if (path == null)
            {
                return null;
            }

            if (Path.DirectorySeparatorChar != '\\')
            {
                return Path.GetDirectoryName(path.Replace('\\', Path.DirectorySeparatorChar));
            }

            return Path.GetDirectoryName(path);
        }

        public static string? Get(string? path)
        {
            if (path == null)
            {
                return null;
            }

            if (Path.DirectorySeparatorChar != '\\')
            {
                return Path.GetFileName(path.Replace('\\', Path.DirectorySeparatorChar));
            }

            return Path.GetFileName(path);
        }

        public static string? Get2(string? path)
        {
            if (path == null)
            {
                return null;
            }

            if (Path.DirectorySeparatorChar != '\\')
            {
                return Path.GetFileNameWithoutExtension(path.Replace('\\', Path.DirectorySeparatorChar));
            }

            return Path.GetFileNameWithoutExtension(path);
        }
    }
}
