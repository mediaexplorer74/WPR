using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class GamerServicesDispatcher
    {

        public static event EventHandler<EventArgs> InstallingTitleUpdate;


        public static void Initialize(IServiceProvider serviceProvider)
        {
        }

        public static void Update()
        {
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
