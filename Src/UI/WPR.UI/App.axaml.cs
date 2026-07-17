using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using WPR.Common;

using System.IO;
using System;
using System.Threading.Tasks;

namespace WPR.UI
{
    public partial class App : Application
    {
        public static Func<IClassicDesktopStyleApplicationLifetime, Task<int>>? DesktopStartup { get; set; }

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
                if (DesktopStartup != null)
                {
                    desktop.MainWindow = new Window
                    {
                        Width = 1,
                        Height = 1,
                        Opacity = 0,
                        ShowInTaskbar = false,
                        CanResize = false
                    };

                    base.OnFrameworkInitializationCompleted();
                    Dispatcher.UIThread.Post(async () =>
                    {
                        int exitCode = 1;
                        try
                        {
                            exitCode = await DesktopStartup(desktop);
                        }
                        finally
                        {
                            desktop.Shutdown(exitCode);
                        }
                    });
                    return;
                }

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
