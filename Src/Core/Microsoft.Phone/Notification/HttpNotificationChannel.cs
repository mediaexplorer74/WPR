using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Phone.Notification;

public sealed class HttpNotificationChannel
{
    public HttpNotificationChannel(string channelName, string serviceName)
    {
        ChannelName = channelName;
        ServiceName = serviceName;
    }

    public string ChannelName { get; }
    public string ServiceName { get; }
    public Uri? ChannelUri { get; private set; }

    public event EventHandler<NotificationChannelUriEventArgs>? ChannelUriUpdated;
    public event EventHandler<NotificationChannelErrorEventArgs>? ErrorOccurred;
    public event EventHandler<HttpNotificationEventArgs>? HttpNotificationReceived;
    public event EventHandler<NotificationEventArgs>? ShellToastNotificationReceived;

    public static HttpNotificationChannel? Find(string channelName) => null;

    public void Open()
    {
        // Microsoft Push Notification Service is not emulated.
    }

    public void Close() => ChannelUri = null;

    public void BindToShellToast()
    {
    }
}

public sealed class NotificationChannelUriEventArgs : EventArgs
{
    public NotificationChannelUriEventArgs(Uri channelUri) => ChannelUri = channelUri;
    public Uri ChannelUri { get; }
}

public sealed class NotificationChannelErrorEventArgs : EventArgs
{
    public NotificationChannelErrorEventArgs(Exception exception) => Exception = exception;
    public Exception Exception { get; }
}

public sealed class HttpNotificationEventArgs : EventArgs
{
    public HttpNotificationEventArgs(HttpNotification notification) => Notification = notification;
    public HttpNotification Notification { get; }
}

public sealed class HttpNotification
{
    public HttpNotification(Stream body) => Body = body;
    public Stream Body { get; }
}

public sealed class NotificationEventArgs : EventArgs
{
    public NotificationEventArgs(IDictionary<string, string>? collection = null) =>
        Collection = collection ?? new Dictionary<string, string>();

    public IDictionary<string, string> Collection { get; }
}
