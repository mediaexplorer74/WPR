using System;

namespace Microsoft.Phone.Info
{
    // Provides extended properties for device information simulation
    public class DeviceExtendedProperties
    {
        // Retrieves a specific device property value by name
        public static Object? GetValue(string property)
        {
            switch (property)
            {
                // Manufacturer name for simulated device
                case "DeviceManufacturer":
                    return "WPRunner";

                // Model name for simulated device
                case "DeviceName":
                    return "WPRunner 2022";

                // Shared version number for firmware and hardware
                case "DeviceFirmwareVersion":
                case "DeviceHardwareVersion":
                    return "8.0.0";

                // Simulated 2GB RAM converted to bytes (2048MB * 1024 * 1024)
                case "DeviceTotalMemory":
                    return 2048UL * 1024 * 1024;

                // Return null for unsupported/unrecognized properties
                default:
                    return null;
            }
        }
    }
}