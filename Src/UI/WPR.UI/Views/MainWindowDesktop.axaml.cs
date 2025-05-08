using Avalonia.Controls;
using System;
using WPR.Common;
using WPR.Models;
using System.Diagnostics;

using Newtonsoft.Json;

namespace WPR.UI.Views
{
    public partial class MainWindowDesktop : Window
    {
        private MainViewNavigator _Navigator;

        public MainWindowDesktop()
        {
            InitializeComponent();

//RnD
#if (true)//!__MOBILE__
#if !__MOBILE__
            MessageBoxUtils.MainWindow = this;
#endif
            ServicesSetup.Start();

            ApplicationLaunchRequest.Incoming += async (sender, args) =>
            {
                Hide();

                _ = NativeUI.NotificationManager.ShowNotification(new DesktopNotifications.Notification()
                {
                    Title = Properties.Resources.LaunchingInProcess,
                    Body = args.Target.Name!,
                    ImagePath = Configuration.Current!.DataPath(args.Target.IconPath)
                }, expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));

                bool runOk = true;

                var test = JsonConvert.SerializeObject(args.Target);
                Debug.WriteLine("[i] " + test);

                string ErrorMessage = "";
                string StackTrace = "";

                try
                {
                    await ApplicationLaunch.Start(args.Target);
                }
                catch (Exception ex)
                {
                    Log.Error(LogCategory.AppList, $"Game run error: \n{ex}");

                    Debug.WriteLine($"[ex] Game run error: \n{ex}");
                    Debug.WriteLine($"Error message: \n{ex.Message}");

                    StackTrace = ex.ToString();
                    ErrorMessage = ex.Message;
                    
                    //RnD
                    runOk = false;
                }

                Show();

                if (!runOk)
                {
                    await MessageBoxUtils.GetMessageDialogResult(
                        title: Properties.Resources.AppRunError + " ("+ ErrorMessage+")",
                        text: Properties.Resources.ExceptionRunApp + ". StackTrace: "+StackTrace,
                        icon: MessageBox.Avalonia.Enums.Icon.Error);
                }
            };
#endif


            _Navigator = new MainViewNavigator();
            _Navigator.SetupNavigation(this.Get<TabControl>("navigationControl"), 
                this.Get<TransitioningContentControl>("contentControl"));
        }
    }
}
