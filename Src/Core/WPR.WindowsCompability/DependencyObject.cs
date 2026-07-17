using WPR.WindowsCompability.Threading;

namespace WPR.WindowsCompability;

public class DependencyObject
{
    public Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
}
