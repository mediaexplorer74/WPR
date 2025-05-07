// Import required namespace for network interface functionality
using System.Net.NetworkInformation;

// Namespace for Windows Phone network information components
namespace Microsoft.Phone.Net.NetworkInformation
{
    // Sealed class providing network interface information
    public sealed class NetworkInterface
    {
        // Static property to get current network interface type
        public static NetworkInterfaceType NetworkInterfaceType
        {
            get
            {
                // Iterate through all available network interfaces on the device
                foreach (System.Net.NetworkInformation.NetworkInterface netInterface
                    in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Check if the current interface is active/operational
                    if (netInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        // Return the network type of the first active interface found
                        // Cast the base framework enum to our local enum type
                        return (NetworkInterfaceType)netInterface.NetworkInterfaceType;
                    }
                }

                // Return 'None' if no active network interfaces found
                return NetworkInterfaceType.None;
            }
        }
    }
}