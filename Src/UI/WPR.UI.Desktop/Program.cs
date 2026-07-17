using Avalonia;
using Avalonia.ReactiveUI;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.IO;

using WPR.WindowsCompability;
using System.Linq;

using WPR.Common;

namespace WPR.UI.Desktop
{
    internal class Program
    {
        internal const string GameHostArgument = "--wpr-game-host";
        private const string DataRootEnvironmentVariable = "WPR_DATA_ROOT";

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static int Main(string[] args)
        {
            string dataRoot = Environment.GetEnvironmentVariable(DataRootEnvironmentVariable)
                ?? Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WPR");
            Configuration.Current = new Configuration(Path.GetFullPath(dataRoot));

            Filesystem.CopyFilesRecursively(Path.Combine(AppContext.BaseDirectory, "Database", "TrueAchievements"),
                Configuration.Current.DataPath("Database\\TrueAchievements"));

            if (!File.Exists(Configuration.Current.DataPath("Database\\achievements.db")))
            {
                File.Copy(Path.Combine(AppContext.BaseDirectory, "Database", "achievements.db"),
                    Configuration.Current.DataPath("Database\\achievements.db"));
            }

            if (!File.Exists(Configuration.Current.DataPath("Database\\applications.db")))
            {
                File.Copy(Path.Combine(AppContext.BaseDirectory, "Database", "applications.db"),
                    Configuration.Current.DataPath("Database\\applications.db"));
            }

            if (args.Length == 2 && args[0] == GameHostArgument)
            {
                if (!Guid.TryParse(args[1], out Guid productId))
                {
                    return 2;
                }

                string normalizedProductId = productId.ToString("D");
                DesktopGameProcessLauncher.RegisterUnhandledExceptionDiagnostics(normalizedProductId);
                App.DesktopStartup = lifetime =>
                    DesktopGameProcessLauncher.RunGameHost(lifetime, normalizedProductId);
            }
            else
            {
                ApplicationLaunchRequest.Launcher = DesktopGameProcessLauncher.Start;
            }

            NativeUI.Initialize();
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                        .UsePlatformDetect()
                        .WithIcons(container => container
                            .Register<FontAwesomeIconProvider>())
                        .LogToTrace()
                        .UseReactiveUI();
        }
    }
}
