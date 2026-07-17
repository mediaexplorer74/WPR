using System;

namespace WPR.WindowsCompability.Threading;

public sealed class Dispatcher
{
    private static readonly Dispatcher Instance = new();

    public static Dispatcher CurrentDispatcher => Instance;

    public DispatcherOperation BeginInvoke(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        action();
        return DispatcherOperation.Completed;
    }

    public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(method);
        method.DynamicInvoke(args);
        return DispatcherOperation.Completed;
    }
}

public sealed class DispatcherOperation
{
    internal static DispatcherOperation Completed { get; } = new();

    private DispatcherOperation()
    {
    }
}
