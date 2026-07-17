using System;

namespace System.Windows.Navigation;

public class NavigationEventArgs : EventArgs
{
    public Uri? Uri { get; init; }
}

public sealed class NavigationFailedEventArgs : EventArgs
{
    public Exception? Exception { get; init; }

    public bool Handled { get; set; }
}

public delegate void NavigationFailedEventHandler(
    object sender, NavigationFailedEventArgs e);
