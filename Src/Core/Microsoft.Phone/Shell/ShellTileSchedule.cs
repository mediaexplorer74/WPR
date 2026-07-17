using System;

namespace Microsoft.Phone.Shell;

public enum UpdateInterval
{
    EveryHour = 0,
    EveryDay = 1,
    EveryWeek = 2
}

public enum UpdateRecurrence
{
    OneTime = 0,
    Interval = 1
}

public sealed class ShellTileSchedule
{
    public UpdateInterval Interval { get; set; }

    public UpdateRecurrence Recurrence { get; set; }

    public Uri? RemoteImageUri { get; set; }

    public DateTime StartTime { get; set; }

    public void Start()
    {
    }

    public void Stop()
    {
    }
}
