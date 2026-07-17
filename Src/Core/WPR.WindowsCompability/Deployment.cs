namespace WPR.WindowsCompability;

public sealed class Deployment : DependencyObject
{
    private static readonly Deployment SharedInstance = new();

    private Deployment()
    {
    }

    public static Deployment Current => SharedInstance;
}
