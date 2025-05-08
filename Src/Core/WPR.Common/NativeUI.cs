using DesktopNotifications;

#if __ANDROID__
  using DesktopNotifications.Android;
//else
//  using DesktopNotifications.Windows;
#endif

using System.Runtime.InteropServices;
using System;
using DesktopNotifications.Windows;

namespace WPR.Common
{
    public static class NativeUI
    {
        public static INotificationManager NotificationManager { get; set; }

        public static void Initialize(object hostControl = null)
        {
#if __ANDROID__
            NotificationManager = new AndroidNotificationManager((hostControl as Android.Content.Context)!);
#else
            NotificationManager = default;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NotificationManager = new WindowsNotificationManager();
            }           
            else 
            {
                throw new PlatformNotSupportedException();
               
            }
#endif
            NotificationManager.Initialize();
        }
    }
}
