namespace Microsoft.Phone.Net.NetworkInformation;

public static class DeviceNetworkInformation
{
    public static bool IsNetworkAvailable =>
        System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
}
