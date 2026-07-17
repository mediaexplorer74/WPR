using System;

namespace WPR.WindowsCompability
{
    public class Application
    {
        private static Application? _Current;
        public event EventHandler<ApplicationUnhandledExceptionEventArgs>? UnhandledException;

        public string? ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                productId = value;
            }
        }
        private ResourceDictionary _Resources;
        private string? productId;
        private readonly Interop.SilverlightHost _host = new();
        private UIElement? _rootVisual;

        internal Application()
        {
            _Resources = new ResourceDictionary();
        }

        public static Application Current 
        {
            get 
            {
                if (_Current == null)
                {
                    _Current = new Application();
                }
                return _Current;
            }
        }

        public ResourceDictionary Resources
        {
            get
            {
                return _Resources;
            }
        }

        public Interop.SilverlightHost Host => _host;

        public UIElement? RootVisual
        {
            get => _rootVisual;
            set => _rootVisual = value;
        }

        public static Resources.StreamResourceInfo? GetResourceStream(Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri);
            string? productId = Current.ProductId;
            if (WPR.Common.Configuration.Current == null || string.IsNullOrWhiteSpace(productId))
            {
                return null;
            }

            string appRoot = System.IO.Path.GetFullPath(WPR.Common.Configuration.Current.DataPath(
                System.IO.Path.Combine("AppData", productId)));
            string relativePath = Uri.UnescapeDataString(uri.OriginalString)
                .Replace('/', System.IO.Path.DirectorySeparatorChar)
                .TrimStart(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            string resourcePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(appRoot, relativePath));
            string rootPrefix = appRoot + System.IO.Path.DirectorySeparatorChar;
            if (!resourcePath.StartsWith(rootPrefix,
                    OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ||
                !System.IO.File.Exists(resourcePath))
            {
                return null;
            }

            return new Resources.StreamResourceInfo(System.IO.File.OpenRead(resourcePath), null);
        }
    }
}
