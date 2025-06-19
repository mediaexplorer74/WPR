using Newtonsoft.Json;
using System.Diagnostics;
using WPR;
using WPR.Common;
using WPR.Tests;
using WPR.Models;

namespace WPRTests
{
    public class Tests
    {
        private void SetupConfiguration()
        {
            var configuration = new Configuration("")
            {
                GamerTag = "WPR"
            };

            Configuration.Current = configuration;

            Configuration.Current.RestoreDefaultDataStoragePath();
            Configuration.Current.Save();
        }

        private static async Task LaunchApplicationAsync(ApplicationLaunchRequestArgs args)
        {
            bool runOk = true;

            var test = JsonConvert.SerializeObject(args.Target);
            Debug.WriteLine("[i] " + test);

            string ErrorMessage = "";
            string StackTrace = "";

            try
            {
                await ApplicationLaunch.Start(args.Target, default);
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.AppList, $"Game run error: \n{ex}");

                Debug.WriteLine($"[ex] Game run error: \n{ex}");
                Debug.WriteLine($"Error message: \n{ex.Message}");

                StackTrace = ex.ToString();
                ErrorMessage = ex.Message;

                runOk = false;
            }

            if (!runOk)
            {
                Log.Error(LogCategory.AppList, "Stacktrace: " + StackTrace);
                Log.Error(LogCategory.AppList, "Error message: " + ErrorMessage);
            }
        }

        [Test]
        public async Task Test1Async()
        {
            SetupConfiguration();

            var application = new Application
            {
                ProductId = "c4f42a26-e64a-e011-854c-00237de2db9e",
                Assembly = "Hydro.dll",
                EntryPoint = "Hydro.CApplication",
                ApplicationType = ApplicationType.XNA
            };

            ApplicationLaunchRequestArgs args = new ApplicationLaunchRequestArgs(application);

            await LaunchApplicationAsync(args);

            Assert.Pass();
        }
    }
}