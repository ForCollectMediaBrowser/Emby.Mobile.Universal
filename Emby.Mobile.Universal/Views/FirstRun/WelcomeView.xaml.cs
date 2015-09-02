using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Helpers;
using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Visibility = Windows.UI.Xaml.Visibility;

namespace Emby.Mobile.Universal.Views.FirstRun
{
    public sealed partial class WelcomeView
    {
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
                
                AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, AboutConnect);
                AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, HasAccountText);
                AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, LoginButton);
                AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, SignUpText);
                AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, SignUpButton);
                AnimationHelper.AddVisibleAnimation(sb, ManualConnectButton);
                AnimationHelper.AddFadeAnim(sb, ManualConnectButton, 1, 500);
                _step = 2;                
            }
            {                
                AnimationHelper.AddFadeAnim(sb, WelcomeLabel, 1, 1000);                
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
                case 1:
                    AnimationHelper.AddSlideAndFadeOutAnimation(sb, WelcomeLabel);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, ServerInfoLabel);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, VisitEmbySiteButton);
                    break;
                case 2:
                    AnimationHelper.AddSlideAndFadeOutAnimation(sb, ServerInfoLabel);
                    AnimationHelper.AddSlideAndFadeOutAnimation(sb, VisitEmbySiteButton);
                    AnimationHelper.AddFadeAnim(sb, NextButton, 0, 100);
                    AnimationHelper.AddCollapseAnimation(sb, NextButton, 100);

                    AnimationHelper.AddSlideAndFadeInAnimation(sb, AboutConnect);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, HasAccountText);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, LoginButton);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, SignUpText);
                    AnimationHelper.AddSlideAndFadeInAnimation(sb, SignUpButton);
                    AnimationHelper.AddVisibleAnimation(sb, ManualConnectButton);
                    AnimationHelper.AddFadeAnim(sb, ManualConnectButton, 1, 500);
                    break;
            }
            sb.Begin();
        }

        private void AnimateToPreviousState()
        {
            var sb = new Storyboard();
            switch (_step)
            {
                case 0:
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, ServerInfoLabel);
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, VisitEmbySiteButton);
                    AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, WelcomeLabel);
                    break;
                case 1:
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, AboutConnect);
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, HasAccountText);
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, LoginButton);
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, SignUpText);
                    AnimationHelper.AddSlideAndFadeOutReverseAnimation(sb, SignUpButton);
                    AnimationHelper.AddFadeAnim(sb, ManualConnectButton, 0, 100);
                    AnimationHelper.AddCollapseAnimation(sb, ManualConnectButton, 100);

                    AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, ServerInfoLabel);
                    AnimationHelper.AddSlideAndFadeInReverseAnimation(sb, VisitEmbySiteButton);
                    AnimationHelper.AddVisibleAnimation(sb, NextButton);
                    AnimationHelper.AddFadeAnim(sb, NextButton, 1, 500);
                    break;
            }
            sb.Begin();
        }

    }
}
