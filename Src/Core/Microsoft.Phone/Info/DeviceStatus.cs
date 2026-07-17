using System;

namespace Microsoft.Phone.Info
{
    public static class DeviceStatus
    {
        public static string DeviceName => "WPRunner Desktop";

        public static string DeviceManufacturer => "WPRunner";

        public static string DeviceFirmwareVersion => Environment.OSVersion.VersionString;

        public static string DeviceHardwareVersion => Environment.Is64BitProcess ? "x64" : "x86";

        public static long DeviceTotalMemory => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
    }
}
