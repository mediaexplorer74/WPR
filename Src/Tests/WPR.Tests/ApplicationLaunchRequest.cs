using WPR.Models;

namespace WPR.Tests
{
    public class ApplicationLaunchRequestArgs : EventArgs
    {
        internal ApplicationLaunchRequestArgs(Application app)
        {
            this.Target = app;
        }

        public Application Target { get; set; }
    }

    public static class ApplicationLaunchRequest
    {
        public static EventHandler<ApplicationLaunchRequestArgs>? Incoming;

        public static void Ask(Application app)
        {
            Incoming?.Invoke(null, new ApplicationLaunchRequestArgs(app));
        }
    }
}