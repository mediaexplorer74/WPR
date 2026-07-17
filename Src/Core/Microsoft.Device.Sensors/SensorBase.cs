using System;

namespace Microsoft.Devices.Sensors
{
    public abstract class SensorBase<TSensorReading> where TSensorReading : ISensorReading
    {
        private TimeSpan _timeBetweenUpdates = TimeSpan.FromMilliseconds(20);

        public event EventHandler<SensorReadingEventArgs<TSensorReading>>? CurrentValueChanged;

        public TimeSpan TimeBetweenUpdates
        {
            get => _timeBetweenUpdates;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _timeBetweenUpdates = value;
            }
        }

        public bool IsDataValid { get; protected set; }

        public virtual void Start()
        {
            IsDataValid = false;
        }

        public virtual void Stop()
        {
            IsDataValid = false;
        }

        protected void OnCurrentValueChanged(SensorReadingEventArgs<TSensorReading> reading)
        {
            CurrentValueChanged?.Invoke(this, reading);
        }
    }
}
