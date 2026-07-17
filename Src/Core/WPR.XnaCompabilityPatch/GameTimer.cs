using System;
using System.Diagnostics;
using System.Threading;

namespace WPR.XnaCompability;

public sealed class GameTimer : IDisposable
{
    private static readonly TimeSpan DefaultInterval = TimeSpan.FromTicks(333333);

    private readonly Stopwatch clock = new();
    private Timer? timer;
    private TimeSpan updateInterval = DefaultInterval;
    private TimeSpan drawInterval = DefaultInterval;
    private TimeSpan lastUpdate;
    private TimeSpan lastDraw;
    private int callbackActive;
    private bool suppressNextDraw;

    public event EventHandler<GameTimerEventArgs>? Update;

    public event EventHandler<GameTimerEventArgs>? Draw;

    public TimeSpan UpdateInterval
    {
        get => updateInterval;
        set
        {
            ValidateInterval(value);
            updateInterval = value;
            UpdateTimerPeriod();
        }
    }

    public TimeSpan DrawInterval
    {
        get => drawInterval;
        set
        {
            ValidateInterval(value);
            drawInterval = value;
            UpdateTimerPeriod();
        }
    }

    public void Start()
    {
        if (timer != null)
        {
            return;
        }

        lastUpdate = TimeSpan.Zero;
        lastDraw = TimeSpan.Zero;
        clock.Restart();
        TimeSpan period = GetTimerPeriod();
        timer = new Timer(Tick, null, period, period);
    }

    public void Stop()
    {
        Timer? current = Interlocked.Exchange(ref timer, null);
        current?.Dispose();
        clock.Stop();
    }

    public void SuppressDraw() => suppressNextDraw = true;

    public void Dispose() => Stop();

    private void Tick(object? state)
    {
        if (Interlocked.Exchange(ref callbackActive, 1) != 0)
        {
            return;
        }

        try
        {
            TimeSpan total = clock.Elapsed;
            if (total - lastUpdate >= updateInterval)
            {
                TimeSpan elapsed = total - lastUpdate;
                lastUpdate = total;
                Update?.Invoke(this, new GameTimerEventArgs(total, elapsed));
            }

            if (total - lastDraw >= drawInterval)
            {
                TimeSpan elapsed = total - lastDraw;
                lastDraw = total;
                if (suppressNextDraw)
                {
                    suppressNextDraw = false;
                }
                else
                {
                    Draw?.Invoke(this, new GameTimerEventArgs(total, elapsed));
                }
            }
        }
        finally
        {
            Volatile.Write(ref callbackActive, 0);
        }
    }

    private void UpdateTimerPeriod()
    {
        TimeSpan period = GetTimerPeriod();
        timer?.Change(period, period);
    }

    private TimeSpan GetTimerPeriod() => updateInterval <= drawInterval ? updateInterval : drawInterval;

    private static void ValidateInterval(TimeSpan value)
    {
        if (value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}

public sealed class GameTimerEventArgs : EventArgs
{
    internal GameTimerEventArgs(TimeSpan totalTime, TimeSpan elapsedTime)
    {
        TotalTime = totalTime;
        ElapsedTime = elapsedTime;
    }

    public TimeSpan TotalTime { get; }

    public TimeSpan ElapsedTime { get; }
}
