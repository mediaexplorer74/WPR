using System;

namespace Microsoft.Phone.Shell
{
    /// <summary>
    /// Provides application lifecycle management and system services for Windows Phone applications.
    /// This class follows a singleton pattern accessible through the Current property.
    /// </summary>
    public class PhoneApplicationService
    {
        /// <summary>
        /// Static constructor initializes the singleton instance.
        /// Ensures PhoneApplicationService is instantiated before first use.
        /// </summary>
        static PhoneApplicationService()
        {
            Current = new PhoneApplicationService();
        }

        /// <summary>
        /// Event triggered when the application is deactivated (sent to background).
        /// Subscribe to handle cleanup operations when app loses focus.
        /// </summary>
        public event EventHandler<DeactivatedEventArgs>? Deactivated;

        /// <summary>
        /// Indicates how the application was started.
        /// Current implementation always returns Launch mode (hardcoded for demonstration).
        /// </summary>
        public StartupMode StartupMode { get => StartupMode.Launch; }

        /// <summary>
        /// Provides access to the singleton instance of PhoneApplicationService.
        /// Set during static initialization and remains constant during app lifetime.
        /// </summary>
        public static PhoneApplicationService? Current { get; private set; }
    }
}