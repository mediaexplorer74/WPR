using System;

namespace Microsoft.Devices.Sensors
{
    public class AccelerometerReadingEventArgs : EventArgs
    {
        public AccelerometerReadingEventArgs(double x, double y, double z, DateTimeOffset? timestamp = null)
        {
            X = x;
            Y = y;
            Z = z;
            Timestamp = timestamp ?? DateTimeOffset.Now;
        }

        public double X { get; }

        public double Y { get; }

        public double Z { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
