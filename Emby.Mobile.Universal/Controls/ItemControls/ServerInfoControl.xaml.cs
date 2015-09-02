using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Emby.Mobile.ViewModels.Entities;

namespace Emby.Mobile.Universal.Controls.ItemControls
{
    public sealed partial class ServerInfoControl
    {
        public ServerInfoControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) => Bindings.Update();
        }

        private ServerInfoViewModel ServerInfo => this.DataContext as ServerInfoViewModel;

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            ShowContextMenu(sender);
        }

        private static void ShowContextMenu(object sender)
        {
            var button = sender as Grid;
            FlyoutBase.ShowAttachedFlyout(button);
        }

        private void ContainingButton_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ShowContextMenu(sender);
        }
    }
}
