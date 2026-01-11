using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using WPR.Common;

using System.IO;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

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
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindowDesktop
                {
                    DataContext = new MainWindowViewModel(),
                };
                
                // Show health check message window to confirm UI flow
                ShowHealthCheckMessage(desktop.MainWindow);
            } else if (ApplicationLifetime is ISingleViewApplicationLifetime mobile)
            {
                mobile.MainView = new MainViewMobile
                {
                    DataContext = new MainWindowViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        private async void ShowHealthCheckMessage(Window mainWindow)
        {
            try
            {
                var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                    title: "WPR Health Check",
                    text: "UI flow confirmed! Application window has been successfully created and displayed.",
                    icon: Icon.Info,
                    windowStartupLocation: WindowStartupLocation.CenterOwner);
                    
                await msgBox.ShowDialog(mainWindow);
            }
            catch (System.Exception ex)
            {
                Log.Error(LogCategory.Startup, $"Health check message failed: {ex.Message}");
            }
        }
    }
}
