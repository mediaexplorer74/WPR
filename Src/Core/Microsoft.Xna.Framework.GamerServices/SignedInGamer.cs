using WPR.WindowsCompability;
using WPR.Common;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class SignedInGamer : Gamer
    {
        private const int DelaySignedInMillis = 2000;
        private static bool FirstSignInSessionDone = false;

        private PlayerIndex _PlayerIndex;

        private GamerPrivileges _GamerPrivileges = new GamerPrivileges();

        private GamerPresence _GamerPresence = new GamerPresence();

        public event EventHandler<EventArgs> AvatarChanged;
        
        public static void Reset()
        {
            FirstSignInSessionDone = false;
        }

        public static event EventHandler<SignedInEventArgs> SignedIn
        {
            add
            {
                if (value != null)
                {
                    if (FirstSignInSessionDone)
                    {
                        value.Invoke(null, new SignedInEventArgs(_SignedInGamers[0]));
                    } else
                    {
                        Task.Delay(DelaySignedInMillis).ContinueWith(previous =>
                        {
                            FirstSignInSessionDone = true;

                            //TODO: handle multiple signed in gamers
                            try
                            {
                                value.Invoke(null, new SignedInEventArgs(_SignedInGamers[0]));
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("[ex] SignedInGamer exception: " + ex.Message);
                            }
                        });
                    }
                }
            }
            remove
            {
            }
        }

        public static event EventHandler<SignedOutEventArgs> SignedOut;

        internal SignedInGamer()
        {
        }

        public IAsyncResult BeginGetAchievements(AsyncCallback? callback, Object? asyncState)
        {
            return Task.Run(async () =>
            {
                List<Achievement> achievementStored = await AchievementContext.Current!.Achievements!
                    .Where(x => x.OwnProductId == Application.Current.ProductId)
                    .ToListAsync();

                if (achievementStored.Count == 0)
                {
                    AchievementCollection collection = await TrueAchievements.Scraper.QueryAchievements(Application.Current!.ProductId!);
                    if (collection.Count != 0)
                    {
                        await AchievementContext.Current!.Achievements!.AddRangeAsync(collection.ToArray());
                        await AchievementContext.Current!.SaveChangesAsync();
                    }

                    if (callback != null)
                    {
                        var compSource = new TaskCompletionSource<AchievementCollection>(asyncState);
                        compSource.SetResult(collection);

                        callback(compSource.Task);
                    }

                    return collection;
                }

                AchievementCollection coll = new AchievementCollection();
                foreach (Achievement achiQueried in achievementStored)
                {
                    coll.Add(achiQueried);
                }

                var completeSource = new TaskCompletionSource<AchievementCollection>(asyncState);
                completeSource.SetResult(coll);

                if (callback != null)
                {
                    callback(completeSource.Task);
                }

                return coll;
            });
        }

        public AchievementCollection EndGetAchievements(IAsyncResult result)
        {
            Log.Error(LogCategory.GamerServices, "Result!");
            Task<AchievementCollection>? collectResult = result as Task<AchievementCollection>;
            return collectResult!.GetAwaiter().GetResult();
        }

        public AchievementCollection GetAchievements() => this.EndGetAchievements(this.BeginGetAchievements(null, null));

        public IAsyncResult BeginAwardAchievement(string achievementKey, AsyncCallback callback,
            object state)
        {
            return Task.Run(async () =>
            {
                List<Achievement> achievements = await AchievementContext.Current!.Achievements!
                    .Where(x => (x.OwnProductId == Application.Current.ProductId) && (x.Key == achievementKey))
                    .ToListAsync();

                if (achievements.Count > 1)
                {
                    Log.Warn(LogCategory.GamerServices, $"More then two achievements with key {achievementKey} exists!");
                }

                if (achievements.Count != 0)
                {
                    foreach (Achievement achievement in achievements)
                    {
                        if (achievement.IsEarned)
                        {
                            continue;
                        }

                        achievement.IsEarned = true;
                        achievement.EarnedOnline = true;
                        achievement.EarnedDateTime = DateTime.Now;
                    }

                    try
                    {
                        await NativeUI.NotificationManager.ShowNotification(new DesktopNotifications.Notification()
                        {
                            Title = Properties.Resources.AchievementUnlocked,
                            Body = $"{achievements[0].GamerScore}G - {achievements[0].Name}",
                            ImagePath = Configuration.Current!.DataPath(achievements[0]._IconPath),
                            SoundUri = "AchievementUnlocked"
                        }, DateTime.Now + TimeSpan.FromDays(1));
                    } catch (Exception ex)
                    {
                        Log.Error(LogCategory.GamerServices, $"Fail to display Achievement notification with exception:\n {ex}");
                    }
                }

                await AchievementContext.Current!.SaveChangesAsync();

                if (callback != null)
                {
                    TaskCompletionSource source = new TaskCompletionSource(state);
                    source.SetResult();

                    callback(source.Task);
                }

                return Task.CompletedTask;
            });
        }

        public void EndAwardAchievement(IAsyncResult result)
        {
        }

        public void AwardAchievement(string achievementKey) => EndAwardAchievement(BeginAwardAchievement(achievementKey, null, null));

        private static readonly FriendCollection EmptyFriends = new FriendCollection();
        private static readonly GameDefaults DefaultGameDefaults = new GameDefaults();
        private static readonly AvatarDescription DefaultAvatar = AvatarDescription.CreateRandom();

        public FriendCollection GetFriends() => EmptyFriends;

        public bool IsFriend(Gamer gamer) => false;

        public AvatarDescription Avatar => DefaultAvatar;

        public GameDefaults GameDefaults => DefaultGameDefaults;

        public bool IsGuest => false;

        public bool IsSignedInToLive
        {
            get
            {
                return true;
            }
        }

        public int PartySize => 0;

        public PlayerIndex PlayerIndex
        {
            get => _PlayerIndex;
            set => _PlayerIndex = value;
        }

        public GamerPresence Presence
        {
            get => _GamerPresence;
            set => _GamerPresence = value;
        }

        public GamerPrivileges Privileges
        {            
            get => _GamerPrivileges;
            set => _GamerPrivileges = value;
        }
    }
   
}
