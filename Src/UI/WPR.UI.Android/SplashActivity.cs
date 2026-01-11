using Android.App;
using Android.Content;
using Android.Content.PM;
using Application = Android.App.Application;

namespace WPR.UI.Android
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {

        protected override void OnResume()
        {
            base.OnResume();
            // Launch MainActivity which initializes Avalonia
            var intent = new Intent(Application.Context, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
        }
    }
}
