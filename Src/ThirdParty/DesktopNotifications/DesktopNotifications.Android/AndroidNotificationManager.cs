using Android.Content;

using AndroidX.Core.App;
using AndroidX.Core.Graphics.Drawable;

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Provider;

namespace DesktopNotifications.Android
{
    public class AndroidNotificationManager : INotificationManager
    {
        public string? LaunchActionId => throw new NotImplementedException();

        public event EventHandler<NotificationActivatedEventArgs>? NotificationActivated;
        public event EventHandler<NotificationDismissedEventArgs>? NotificationDismissed;

        private NotificationManagerCompat _NofManagerCompat;

        private int _NofId = 0;
        private int _NofChannelCounter = 0;

        private static string _ChannelName = "DesktopNotifications";

        private Context _Context;
        private Dictionary<string, string> _NofChannels;

        public AndroidNotificationManager(Context context)
        {
            _NofChannels = new Dictionary<string, string>();

            _Context = context;
            _NofManagerCompat = NotificationManagerCompat.From(_Context);
        }

        private string GetNotificationChannelForAudio(string audioPath)
        {
            string audioPathNormalized = audioPath;

            if (audioPathNormalized != null)
            {
                if (global::Android.Net.Uri.Parse(audioPathNormalized) == null)
                {
                    audioPathNormalized = "";
                }
            }
            
            if (_NofChannels.ContainsKey(audioPathNormalized!))
            {
                return _NofChannels[audioPathNormalized!];
            }

            var _NofChannel = new NotificationChannel($"DesktopNotificationChannelV10_{_NofChannelCounter++}",
                _ChannelName, NotificationImportance.Max);

            _NofChannel.Description = _ChannelName;

            if (audioPathNormalized != "")
            {
                AudioAttributes att = new AudioAttributes.Builder()!
                        .SetUsage(AudioUsageKind.Notification)!
                        .SetContentType(AudioContentType.Speech)!
                        .Build()!;

                _NofChannel.SetSound(global::Android.Net.Uri.Parse(audioPathNormalized), att);
            }

            _NofManagerCompat.CreateNotificationChannel(_NofChannel);
            _NofChannels.Add(audioPathNormalized!, _NofChannel.Id!);

            return _NofChannel.Id!;
        }

        public void Dispose()
        {
            foreach (var channel in _NofChannels.Values)
            {
                _NofManagerCompat.DeleteNotificationChannel(channel);
            }
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task ScheduleNotification(Notification notification, DateTimeOffset deliveryTime, DateTimeOffset? expirationTime = null)
        {
            throw new NotImplementedException();
        }

        public Task ShowNotification(Notification notification, DateTimeOffset? expirationTime = null)
        {
            global::Android.Net.Uri? uriSound = RingtoneManager.GetActualDefaultRingtoneUri(_Context, RingtoneType.Notification);
            string? pathComplete = (uriSound != null) ? uriSound.Path : null;
            if (notification.SoundUri != null)
            {
                pathComplete = $"android.resource://{_Context.ApplicationInfo!.PackageName}/raw/{notification.SoundUri.ToLower()}";
            }

            NotificationCompat.Builder builder = (Build.VERSION.SdkInt >= BuildVersionCodes.O) ?
                new NotificationCompat.Builder(_Context, GetNotificationChannelForAudio(pathComplete ?? "")) :
                new NotificationCompat.Builder(_Context);

            builder.SetContentTitle(notification.Title)
                .SetContentText(notification.Body)
                .SetDefaults(NotificationCompat.DefaultVibrate)
                .SetPriority(NotificationCompat.PriorityMax);

            bool iconSet = false;

            if (notification.ImagePath != null)
            {
                Bitmap? icon = BitmapFactory.DecodeFile(notification.ImagePath);
                if (icon != null)
                {
                    builder.SetLargeIcon(icon);

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        builder.SetSmallIcon(IconCompat.CreateWithBitmap(icon));
                    } else
                    {
                        builder.SetSmallIcon(_Context.ApplicationInfo!.Icon);
                    }

                    iconSet = true;
                }
            }

            if (!iconSet)
            {
                builder.SetSmallIcon(_Context.ApplicationInfo!.Icon);
            }

            if (expirationTime != null)
            {
                long duration = (long)(expirationTime - DateTime.Now)!.Value.TotalMilliseconds;
                builder.SetTimeoutAfter(duration);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                if (pathComplete != null)
                {
                    builder.SetSound(global::Android.Net.Uri.Parse(pathComplete));
                }
            }

            _NofManagerCompat.Notify(_NofId++, builder.Build());
            return Task.CompletedTask;
        }
    }
}