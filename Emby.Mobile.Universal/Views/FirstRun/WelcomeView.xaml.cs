namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class WelcomeView
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.Navigate<DownloadServerView>();
        }
    }
}
