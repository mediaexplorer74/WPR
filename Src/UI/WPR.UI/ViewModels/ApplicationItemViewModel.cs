using Avalonia.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using ReactiveUI;
using System;

using WPR.Models;
using WPR.Common;
using System.Reactive;

namespace WPR.UI.ViewModels
{
    public class ApplicationItemViewModel : ViewModelBase
    {
        private Application _App;
        private Bitmap? _Icon;

        public int IconSize => 90;
        public int Height => 160;

        public ReactiveCommand<Unit, Unit> RunAppCommand { get; }
        public ReactiveCommand<Unit, Unit> UninstallAppCommand { get; }

        // Event that will be triggered when app needs to be uninstalled
        public event EventHandler<ApplicationItemViewModel>? UninstallRequested;

        public ApplicationItemViewModel(Application app)
        {
            _App = app;
            RunAppCommand = ReactiveCommand.Create(() => RunApp());
            UninstallAppCommand = ReactiveCommand.Create(() => UninstallApp());
        }

        internal Application App => _App;
        
        // Property to expose the application model for easier access
        public Application Model => _App;

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
                _Icon = default;

                if (_Icon == null)
                {
                    try
                    {
                        var iconpath = Configuration.Current!.DataPath(_App.IconPath);
                        var fs = new FileStream(iconpath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _Icon = Bitmap.DecodeToWidth(fs,
                            IconSize);
                    }
                    catch { }
                }

                return _Icon;
            }
        }

        private void RunApp()
        {
            // Logic to run the application
            WPR.UI.ApplicationLaunchRequest.Ask(_App);
        }

        private void UninstallApp()
        {
            // Trigger the uninstall event so the parent view model can handle it
            UninstallRequested?.Invoke(this, this);
        }
    }
}
