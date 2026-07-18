namespace WPR.WindowsCompability.Interop;

public sealed class SilverlightHost
{
    private readonly Settings _settings = new();
    private readonly Content _content = new();

    public Settings Settings => _settings;

    public Content Content => _content;
}

public sealed class Content
{
    public double ActualWidth => 800;

    public double ActualHeight => 480;
}

public sealed class Settings
{
    public bool EnableFrameRateCounter { get; set; }
}
