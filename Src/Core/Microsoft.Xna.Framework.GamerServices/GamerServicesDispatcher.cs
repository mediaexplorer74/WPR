using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class GamerServicesDispatcher
    {
        private static readonly ConcurrentQueue<Action> PendingActions = new();

        public static event EventHandler<EventArgs> InstallingTitleUpdate;


        public static void Initialize(IServiceProvider serviceProvider)
        {
        }

        public static void Update()
        {
            while (PendingActions.TryDequeue(out Action? action))
            {
                action();
            }
        }

        internal static void Schedule(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            PendingActions.Enqueue(action);
        }

        internal static void Reset()
        {
            while (PendingActions.TryDequeue(out _))
            {
            }
        }

        public static bool IsInitialized => true;

        private static IntPtr _windowHandle;

        public static IntPtr WindowHandle
        {
            get => _windowHandle;
            set => _windowHandle = value;
        }
    }
}
