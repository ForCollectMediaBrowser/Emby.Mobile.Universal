using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class DownloadServerView : Page
    {
        public DownloadServerView()
        {
            InitializeComponent();
        }

        private void VisitEmbySiteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO Launch Emby website
        }

        private void NextButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppServices.NavigationService.Navigate<FirstConnectView>();
        }
    }
}
