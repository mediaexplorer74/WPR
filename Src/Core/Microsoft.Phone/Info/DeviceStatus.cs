using System;
using System.Diagnostics;

namespace Microsoft.Phone.Info
{
    public static class DeviceStatus
    {
        public static string DeviceName => "WPRunner Desktop";

        public static string DeviceManufacturer => "WPRunner";

        public static string DeviceFirmwareVersion => Environment.OSVersion.VersionString;

        public static string DeviceHardwareVersion => Environment.Is64BitProcess ? "x64" : "x86";

        public static long DeviceTotalMemory => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

        public static long ApplicationCurrentMemoryUsage
        {
            get
            {
                using Process process = Process.GetCurrentProcess();
                return process.WorkingSet64;
            }
        }

        public static long ApplicationPeakMemoryUsage
        {
            get
            {
                using Process process = Process.GetCurrentProcess();
                return process.PeakWorkingSet64;
            }
        }

        public static long ApplicationMemoryUsageLimit => GetMemoryLimit();

        internal static long GetMemoryLimit()
        {
            long available = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
            return available > 0 ? available : 512L * 1024 * 1024;
        }
    }
}
