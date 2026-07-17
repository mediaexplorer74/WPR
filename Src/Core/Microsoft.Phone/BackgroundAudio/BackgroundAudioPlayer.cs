using System;

namespace Microsoft.Phone.BackgroundAudio;

public sealed class BackgroundAudioPlayer
{
    private static readonly BackgroundAudioPlayer SharedInstance = new();

    private BackgroundAudioPlayer()
    {
    }

    public static BackgroundAudioPlayer Instance => SharedInstance;

    public event EventHandler? PlayStateChanged;
}
