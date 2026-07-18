using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WPR.Common;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class LeaderboardReader
    {
        private ReadOnlyCollection<LeaderboardEntry>? _Entries;
        public ReadOnlyCollection<LeaderboardEntry>? Entries => this._Entries;

        public LeaderboardIdentity LeaderboardIdentity { get; private set; }

        public int PageStart { get; private set; }

        public LeaderboardReader()
            : this(default, 0)
        {
        }

        private LeaderboardReader(LeaderboardIdentity leaderboardIdentity, int pageStart)
        {
            _Entries = new ReadOnlyCollection<LeaderboardEntry>(new List<LeaderboardEntry>());
            LeaderboardIdentity = leaderboardIdentity;
            PageStart = pageStart;
        }

        public IAsyncResult BeginPageDown(AsyncCallback callback, object asyncState)
        {
            return CompletePage(callback, asyncState);
        }
        public IAsyncResult BeginPageUp(AsyncCallback callback, object asyncState)
        {
            return CompletePage(callback, asyncState);
        }
        public static IAsyncResult BeginRead(LeaderboardIdentity leaderb,
            int pageStart, int pageSize, AsyncCallback callback, object asyncState)
        {
            return CompleteRead(leaderb, pageStart, callback, asyncState);
        }

        public static LeaderboardReader Read(
            LeaderboardIdentity leaderboardId, int pageStart, int pageSize)
        {
            return EndRead(BeginRead(
                leaderboardId, pageStart, pageSize, callback: null!, asyncState: null!));
        }

        public static IAsyncResult BeginRead(
          LeaderboardIdentity leaderboardId,
          Gamer pivotGamer,
          int pageSize,
          AsyncCallback callback,
          object asyncState)
        {
            return CompleteRead(leaderboardId, 0, callback, asyncState);
        }

        public static LeaderboardReader Read(
            LeaderboardIdentity leaderboardId, Gamer pivotGamer, int pageSize)
        {
            return EndRead(BeginRead(
                leaderboardId, pivotGamer, pageSize, callback: null!, asyncState: null!));
        }

        public static IAsyncResult BeginRead(
          LeaderboardIdentity leaderboardId,
          IEnumerable<Gamer> gamers,
          Gamer pivotGamer,
          int pageSize,
          AsyncCallback callback,
          object asyncState)
        {
            return CompleteRead(leaderboardId, 0, callback, asyncState);
        }

        public static LeaderboardReader Read(
            LeaderboardIdentity leaderboardId,
            IEnumerable<Gamer> gamers,
            Gamer pivotGamer,
            int pageSize)
        {
            return EndRead(BeginRead(
                leaderboardId, gamers, pivotGamer, pageSize,
                callback: null!, asyncState: null!));
        }

        private static IAsyncResult CompleteRead(
            LeaderboardIdentity leaderboardIdentity,
            int pageStart,
            AsyncCallback? callback,
            object? asyncState)
        {
            var source = new TaskCompletionSource<LeaderboardReader>(asyncState,
                TaskCreationOptions.RunContinuationsAsynchronously);
            source.SetResult(new LeaderboardReader(leaderboardIdentity, pageStart));
            callback?.Invoke(source.Task);
            return source.Task;
        }

        public static LeaderboardReader EndRead(IAsyncResult result)
        {
            return ((Task<LeaderboardReader>)result).GetAwaiter().GetResult();
        }

        private IAsyncResult CompletePage(AsyncCallback? callback, object? asyncState)
        {
            var source = new TaskCompletionSource<LeaderboardReader>(asyncState,
                TaskCreationOptions.RunContinuationsAsynchronously);
            source.SetResult(this);
            callback?.Invoke(source.Task);
            return source.Task;
        }

        public void EndPageDown(IAsyncResult result) =>
            ((Task<LeaderboardReader>)result).GetAwaiter().GetResult();

        public void EndPageUp(IAsyncResult result) =>
            ((Task<LeaderboardReader>)result).GetAwaiter().GetResult();

        //public IAsyncResult TotalLeaderboardSize()
        //{
        //    return StubUtils.ForeverTask;
        //}


        public Int32 TotalLeaderboardSize => _Entries?.Count ?? 0;

        public bool CanPageDown => false;

        public bool CanPageUp => false;
    }
}
