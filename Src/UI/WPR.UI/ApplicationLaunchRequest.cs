using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.UI
{
    public class ApplicationLaunchRequestArgs : EventArgs
    {
        internal ApplicationLaunchRequestArgs(Models.Application app)
        {
            this.Target = app;
        }

        public Models.Application Target { get; set; }
    }

    public static class ApplicationLaunchRequest
    {
        public static EventHandler<ApplicationLaunchRequestArgs>? Incoming;
        public static Func<Models.Application, Task> Launcher { private get; set; } =
            app => ApplicationLaunch.Start(app, default);

        public static Task Launch(Models.Application app)
        {
            return Launcher(app);
        }

        public static void Ask(Models.Application app)
        {
            try
            {
                Incoming?.Invoke(null, new ApplicationLaunchRequestArgs(app));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] ApplicationLaunchRequest (Ask) error: " + ex.Message);
#if __ANDROID__
                WPR.Common.Log.Error(WPR.Common.LogCategory.AppList, $"ApplicationLaunchRequest failed: {ex}");
#endif
            }
        }
    }
}
