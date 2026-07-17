using System;
using System.Threading;

namespace WPR.WindowsCompability.Threading;

public sealed class DispatcherTimer : IDisposable
{
    private Timer? _timer;
    private TimeSpan _interval = TimeSpan.FromSeconds(1);

    public event EventHandler? Tick;

    public TimeSpan Interval
    {
        get => _interval;
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _interval = value;
            if (IsEnabled)
            {
                _timer!.Change(_interval, _interval);
            }
        }
    }

    public bool IsEnabled => _timer != null;

    public void Start()
    {
        _timer ??= new Timer(_ => Tick?.Invoke(this, EventArgs.Empty), null, _interval, _interval);
    }

    public void Stop()
    {
        _timer?.Dispose();
        _timer = null;
    }

    public void Dispose() => Stop();
}
