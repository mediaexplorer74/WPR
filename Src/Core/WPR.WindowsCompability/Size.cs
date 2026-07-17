namespace WPR.WindowsCompability;

public struct Size
{
    public Size(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double Width { get; set; }
    public double Height { get; set; }
}
