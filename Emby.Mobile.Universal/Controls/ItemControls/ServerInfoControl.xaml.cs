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
            this.InitializeComponent(); 
        }

        private ServerInfoViewModel ServerInfo => this.DataContext as ServerInfoViewModel;

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            var button = sender as Button;
            FlyoutBase.ShowAttachedFlyout(button);
        }
    }
}
