using System;
using System.Diagnostics;

namespace Microsoft.Phone.Info
{
    public static class UserExtendedProperties
    {
        public static Object GetValue(string propertyName)
        {
            if (propertyName == null)
            {
                //throw new ArgumentNullException(
                //    "Null property name in retrieving user extended properties' value!");
                return default; // RnD
            }

            switch (propertyName)
            {
                case "ANID":
                    return "123456789";

                default:
                    //RnD
                    Debug.WriteLine("[ex] GetValue - Unknown property name: " + propertyName);

                    //throw new ArgumentException(
                    //  "[error] GetValue - Unknown property name: "
                    //  + propertyName);
                    return default; 
            }
        }

        public static bool TryGetValue(string propertyName, out Object propertyValue)
        {
            if (propertyName == null)
            {
                //RnD
                Debug.WriteLine("[ex] TryGetValue - Unknown property name: " + propertyName);
                //throw new ArgumentNullException(
                //    "Null property name in retrieving user extended properties' value!");
                propertyName = default; 
            }

            propertyValue = null;

            //RnD
            try
            {
                propertyValue = GetValue(propertyName);
            } 
            catch
            {
                return false;
            }

            return true;
        }

        public static bool DeviceStatus()
        {
            //RnD
            return true;
        }
    }
}
