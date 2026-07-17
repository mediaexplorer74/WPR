using System;

namespace Microsoft.Phone.Info
{
    public class DeviceExtendedProperties
    {
        public static bool TryGetValue(string propertyName, out Object propertyValue)
        {
            object? value = GetValue(propertyName);
            propertyValue = value!;
            return value != null;
        }

        public static Object? GetValue(string property)
        {
            switch (property)
            {
                case "DeviceManufacturer":
                    return "WPRunner";

                case "DeviceName":
                    return "WPRunner 2022";

                case "DeviceFirmwareVersion":
                case "DeviceHardwareVersion":
                    return "8.0.0";

                case "DeviceTotalMemory":
                    return DeviceStatus.DeviceTotalMemory;

                case "ApplicationCurrentMemoryUsage":
                    return DeviceStatus.ApplicationCurrentMemoryUsage;

                case "ApplicationPeakMemoryUsage":
                    return DeviceStatus.ApplicationPeakMemoryUsage;

                case "ApplicationMemoryUsageLimit":
                case "ApplicationWorkingSetLimit":
                    return DeviceStatus.ApplicationMemoryUsageLimit;

                default:
                    return null;
            }
        }
    }
}
