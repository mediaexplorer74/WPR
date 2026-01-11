using Avalonia;
using Avalonia.ReactiveUI;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;
using System.IO;
using System.Diagnostics;

using WPR.WindowsCompability;
using System.Linq;

using WPR.Common;

namespace WPR.UI.Desktop
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting WPR application...");
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting WPR application...");
            Log.Info(LogCategory.Startup, "Starting WPR application...");

            Configuration.Current = new Configuration(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WPR"));

            Filesystem.CopyFilesRecursively(Path.Combine(Directory.GetCurrentDirectory(), "Database\\TrueAchievements"),
                Configuration.Current.DataPath("Database\\TrueAchievements"));

            if (!File.Exists(Configuration.Current.DataPath("Database\\achievements.db")))
            {
                File.Copy("Database\\achievements.db", Configuration.Current.DataPath("Database\\achievements.db"));
            }

            if (!File.Exists(Configuration.Current.DataPath("Database\\applications.db")))
            {
                File.Copy("Database\\applications.db", Configuration.Current.DataPath("Database\\applications.db"));
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Database files initialized.");
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Database files initialized.");
            Log.Info(LogCategory.Startup, "Database files initialized.");

            NativeUI.Initialize();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Native UI initialized.");
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Native UI initialized.");
            Log.Info(LogCategory.Startup, "Native UI initialized.");

            var appBuilder = BuildAvaloniaApp();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Avalonia AppBuilder configured.");
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Avalonia AppBuilder configured.");
            Log.Info(LogCategory.Startup, "Avalonia AppBuilder configured.");

            appBuilder.StartWithClassicDesktopLifetime(args);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Avalonia application started.");
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Avalonia application started.");
            Log.Info(LogCategory.Startup, "Avalonia application started.");
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .WithIcons(container => container
                    .Register<FontAwesomeIconProvider>());
    }
}
