using System;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls;

public sealed class WebBrowser
{
    public event EventHandler<NavigationEventArgs>? Navigated;

    public event NavigationFailedEventHandler? NavigationFailed;

    public void NavigateToString(string html)
    {
        Navigated?.Invoke(this, new NavigationEventArgs());
    }
}
