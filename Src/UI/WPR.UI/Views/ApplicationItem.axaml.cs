using Avalonia.Controls;
using Avalonia.Input;
using WPR.UI.ViewModels;

namespace WPR.UI.Views
{
    public partial class ApplicationView : UserControl
    {
        public ApplicationView()
        {
            InitializeComponent();
            AddHandler(PointerReleasedEvent, OnPointerReleased, Avalonia.Interactivity.RoutingStrategies.Bubble);
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton != MouseButton.Left)
            {
                return;
            }

            if (e.Source is Button)
            {
                return;
            }

            if (DataContext is ApplicationItemViewModel vm)
            {
                WPR.UI.ApplicationLaunchRequest.Ask(vm.Model);
            }
        }
    }
}
