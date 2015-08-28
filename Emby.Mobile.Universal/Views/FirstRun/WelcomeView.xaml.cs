using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class WelcomeView : Page
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppServices.NavigationService.Navigate<DownloadServerView>();
        }
    }
}
