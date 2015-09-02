using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.Universal.Helpers;
using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Visibility = Windows.UI.Xaml.Visibility;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class WelcomeView
    {
        private const int WelcomePage = 0;
        private const int ServerDescriptionPage = 1;
        private const int AboutConnectPage = 2;

        private int _step = 0;

        public WelcomeView()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPressed(object sender, NavigationServiceBackKeyPressedEventArgs e)
        {
            if (_step > 0)
            {
                _step -= 1;
                AnimateToPreviousState();
                e.Behavior = NavigationServiceBackKeyPressedBehavior.DoNothing;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var sb = new Storyboard();
            if (e.NavigationMode == NavigationMode.Back)
            {
                WelcomeLabel.Visibility = Visibility.Collapsed;
                ServerInfoLabel.Visibility = Visibility.Collapsed;
                VisitEmbySiteButton.Visibility = Visibility.Collapsed;
                NextButton.Visibility = Visibility.Collapsed;

                sb.AddSlideAndFadeInReverseAnimation(AboutConnect);
                sb.AddSlideAndFadeInReverseAnimation(HasAccountText);
                sb.AddSlideAndFadeInReverseAnimation(LoginButton);
                sb.AddSlideAndFadeInReverseAnimation(SignUpText);
                sb.AddSlideAndFadeInReverseAnimation(SignUpButton);
                sb.AddVisibleAnimation(ManualConnectButton);
                sb.AddFadeAnim(ManualConnectButton, 1, 500);
                _step = 2;                
            }
            {
                sb.AddFadeAnim(WelcomeLabel, 1, 1000);                
            }
            sb.Begin();
            base.OnNavigatedTo(e);
        }

        private void NextButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _step += 1;
            AnimateToNextState();
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
            NavigationService.NavigateToLocalServerSelection();
        }

        private void VisitEmbySiteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {            
            AppServices.LauncherService.LaunchUriAsync("http://www.emby.media/");
        }

        private void AnimateToNextState()
        {
            var sb = new Storyboard();
            switch (_step)
            {
                case ServerDescriptionPage:
                    sb.AddSlideAndFadeOutAnimation(WelcomeLabel);
                    sb.AddSlideAndFadeInAnimation(ServerInfoLabel);
                    sb.AddSlideAndFadeInAnimation(VisitEmbySiteButton);
                    break;
                case AboutConnectPage:
                    sb.AddSlideAndFadeOutAnimation(ServerInfoLabel);
                    sb.AddSlideAndFadeOutAnimation(VisitEmbySiteButton);
                    sb.AddFadeAnim(NextButton, 0, 100);
                    sb.AddCollapseAnimation(NextButton, 100);

                    sb.AddSlideAndFadeInAnimation(AboutConnect);
                    sb.AddSlideAndFadeInAnimation(HasAccountText);
                    sb.AddSlideAndFadeInAnimation(LoginButton);
                    sb.AddSlideAndFadeInAnimation(SignUpText);
                    sb.AddSlideAndFadeInAnimation(SignUpButton);
                    sb.AddVisibleAnimation(ManualConnectButton);
                    sb.AddFadeAnim(ManualConnectButton, 1, 500);
                    break;
            }
            sb.Begin();
        }

        private void AnimateToPreviousState()
        {
            var sb = new Storyboard();
            switch (_step)
            {
                case WelcomePage:
                    sb.AddSlideAndFadeOutReverseAnimation(ServerInfoLabel);
                    sb.AddSlideAndFadeOutReverseAnimation(VisitEmbySiteButton);
                    sb.AddSlideAndFadeInReverseAnimation(WelcomeLabel);
                    break;
                case ServerDescriptionPage:
                    sb.AddSlideAndFadeOutReverseAnimation(AboutConnect);
                    sb.AddSlideAndFadeOutReverseAnimation(HasAccountText);
                    sb.AddSlideAndFadeOutReverseAnimation(LoginButton);
                    sb.AddSlideAndFadeOutReverseAnimation(SignUpText);
                    sb.AddSlideAndFadeOutReverseAnimation(SignUpButton);
                    sb.AddFadeAnim(ManualConnectButton, 0, 100);
                    sb.AddCollapseAnimation(ManualConnectButton, 100);

                    sb.AddSlideAndFadeInReverseAnimation(ServerInfoLabel);
                    sb.AddSlideAndFadeInReverseAnimation(VisitEmbySiteButton);
                    sb.AddVisibleAnimation(NextButton);
                    sb.AddFadeAnim(NextButton, 1, 500);
                    break;
            }
            sb.Begin();
        }

    }
}
