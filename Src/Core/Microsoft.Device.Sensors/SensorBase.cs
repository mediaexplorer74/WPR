using System;

namespace Microsoft.Devices.Sensors
{
    public abstract class SensorBase<TSensorReading> : IDisposable where TSensorReading : ISensorReading
    {
        private TimeSpan _timeBetweenUpdates = TimeSpan.FromMilliseconds(20);
        private TSensorReading _currentValue = default!;

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

        public TSensorReading CurrentValue => _currentValue;

        public virtual void Start()
        {
            IsDataValid = false;
        }

        public virtual void Stop()
        {
            IsDataValid = false;
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        protected void OnCurrentValueChanged(SensorReadingEventArgs<TSensorReading> reading)
        {
            _currentValue = reading.SensorReading;
            CurrentValueChanged?.Invoke(this, reading);
        }
    }
}
