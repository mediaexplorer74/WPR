namespace WPR.WindowsCompability;

public class UIElement : DependencyObject
{
    public Visibility Visibility { get; set; } = Visibility.Visible;

    public virtual void UpdateLayout()
    {
    }
}

public class FrameworkElement : UIElement
{
    public double Height { get; set; }

    public Media.Thickness Margin { get; set; }

    public double Width { get; set; }
}

public enum Visibility
{
    Visible = 0,
    Collapsed = 1
}
