namespace WPR.WindowsCompability.Interop;

public sealed class SilverlightHost
{
    private readonly Settings _settings = new();

    public Settings Settings => _settings;
}

public sealed class Settings
{
    public bool EnableFrameRateCounter { get; set; }
}
