using Emby.Mobile.Universal.Services;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class FirstConnectView
    {
        public FirstConnectView()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.NavigateToEmbyConnectSignUp();
        }

        private void LoginButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.NavigateToEmbyConnect();
        }

        private void ManualConnectButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.NavigateToManualServerEntry();
        }
    }
}
