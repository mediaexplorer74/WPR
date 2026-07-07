using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using WPR.Common;

using System.IO;

namespace WPR.UI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            RequestedThemeVariant = ThemeVariant.Dark;

            var mainViewModel = new MainWindowViewModel();
            var mainView = new MainViewMobile { DataContext = mainViewModel };

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindowDesktop
                {
                    DataContext = mainViewModel,
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = mainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
