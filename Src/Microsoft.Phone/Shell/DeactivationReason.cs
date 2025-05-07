// Namespace for Windows Phone shell-related functionalities
namespace Microsoft.Phone.Shell
{
    // Enumeration representing different application deactivation reasons
    public enum DeactivationReason
    {
        // User-initiated action (e.g. pressing back button, closing app)
        UserAction,

        // Device entered power saving mode
        PowerSavingModeOn,

        // Application-initiated deactivation
        ApplicationAction,

        // System resource constraints or unavailability
        ResourceUnavailable,
    }
}