using DesktopNotifications;
using DesktopNotifications.Windows;

#if __ANDROID__
using DesktopNotifications.Android;
#endif

using System.Runtime.InteropServices;
using System;

namespace WPR.Common
{
    public static class NativeUI
    {
        public static INotificationManager NotificationManager { get; set; }

        public static void Initialize(object hostControl = null)
        {
//#if __ANDROID__
//            NotificationManager = new AndroidNotificationManager((hostControl as Android.Content.Context)!);
//#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // FreeDesktopNotificationManager is not available in this build; leave null
                NotificationManager = null;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NotificationManager = new WindowsNotificationManager();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // AppleNotificationManager is not available in this build; leave null
                NotificationManager = null;
            }
            else
            {
                // Unknown platform
                NotificationManager = null;
            }
//#endif
            //NotificationManager.Initialize();
        }
    }
}
