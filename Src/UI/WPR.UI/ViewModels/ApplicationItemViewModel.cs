using Avalonia.Media.Imaging;
using System.IO;

using WPR.Models;
using WPR.Common;
using System;

namespace WPR.UI.ViewModels
{
    public class ApplicationItemViewModel : ViewModelBase
    {
        private Application _App;
        private Bitmap? _Icon;

        public int IconSize => 90;
        public int Height => 160;

        public ApplicationItemViewModel(Application app)
        {
            _App = app;
        }

        internal Application App => _App;

        public string? Name => _App.Name;
        public string? Tooltip
        {
            get
            {
                return (_App.Description.Length == 0) ? _App.Name : $"{_App.Name}\n\n{_App.Description}";
            }
        }

        public Bitmap Icon
        {
            get
            {
                if (_Icon == null)
                {
                    var iconRelative = _App.IconPath ?? string.Empty;
                    var iconFull = Configuration.Current!.DataPath(iconRelative);

                    if (File.Exists(iconFull))
                    {
                        try
                        {
                            using var fs = new FileStream(iconFull, FileMode.Open, FileAccess.Read, FileShare.Read);
                            _Icon = Bitmap.DecodeToWidth(fs, IconSize);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(/*LogCategory.UI*/default, $"Failed to load icon '{iconFull}': {ex}");
                            _Icon = LoadDefaultIcon();
                        }
                    }
                    else
                    {
                        Log.Error(/*LogCategory.UI*/default, $"Icon not found: {iconFull}");
                        _Icon = LoadDefaultIcon();
                    }
                }

                return _Icon;
            }
        }

        private static readonly byte[] OnePixelPng =
            Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAn0B9lqzWgAAAABJRU5ErkJggg==");

        private  Bitmap LoadDefaultIcon()
        {
            var bytes = Properties.Resources.DefaultIconPng;
            if (bytes == null || bytes.Length == 0)
            {
                Bitmap DefIcon = default;

                try
                {
                    // use a known-valid 1x1 PNG and decode to the desired width
                    DefIcon = Bitmap.DecodeToWidth(new MemoryStream(OnePixelPng), IconSize); 
                }
                catch { }

                return DefIcon;
            }
            return Bitmap.DecodeToWidth(new MemoryStream(bytes), IconSize);
        }
    }
}
