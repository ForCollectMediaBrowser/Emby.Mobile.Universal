using Cimbalino.Toolkit.Services;
using Emby.Mobile.Universal.Helpers;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

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
            AnimationHelper.AddFadeAnim(sb, WelcomeLabel, 1, 1000);
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
            //TODO Launch Emby website
        }

        private void AnimateToNextState()
        {
            var sb = new Storyboard();
            switch (_step)
            {                
                case 1:
                    AnimationHelper.AddFadeAnim(sb, WelcomeLabel, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, ServerInfoLabel, 1, 1000);
                    AnimationHelper.AddFadeAnim(sb, VisitEmbySiteButton, 1, 1000);
                    AnimationHelper.AddVisibleAnimation(sb, VisitEmbySiteButton);

                    break;
                case 2:
                    AnimationHelper.AddFadeAnim(sb, ServerInfoLabel, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, VisitEmbySiteButton, 0, 500);
                    AnimationHelper.AddCollapseAnimation(sb, NextButton, 100);
                    AnimationHelper.AddCollapseAnimation(sb, VisitEmbySiteButton, 100);
                    AnimationHelper.AddFadeAnim(sb, AboutConnect, 1, 1000);
                    AnimationHelper.AddFadeAnim(sb, HasAccountText, 1, 1000);
                    AnimationHelper.AddVisibleAnimation(sb, LoginButton, 1);
                    AnimationHelper.AddFadeAnim(sb, LoginButton, 1, 1000);
                    AnimationHelper.AddFadeAnim(sb, SignUpText, 1, 1000);
                    AnimationHelper.AddVisibleAnimation(sb, SignUpButton);
                    AnimationHelper.AddFadeAnim(sb, SignUpButton, 1, 1000);
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
                    AnimationHelper.AddFadeAnim(sb, ServerInfoLabel, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, VisitEmbySiteButton, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, WelcomeLabel, 1, 1000);
                    break;
                case 1:
                    AnimationHelper.AddFadeAnim(sb, AboutConnect, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, HasAccountText, 0, 500);
                    AnimationHelper.AddCollapseAnimation(sb, LoginButton, 500);
                    AnimationHelper.AddFadeAnim(sb, LoginButton, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, SignUpText, 0, 500);
                    AnimationHelper.AddFadeAnim(sb, SignUpButton, 0, 500);
                    AnimationHelper.AddCollapseAnimation(sb, SignUpButton, 500);
                    AnimationHelper.AddCollapseAnimation(sb, ManualConnectButton, 100);
                    AnimationHelper.AddFadeAnim(sb, ManualConnectButton, 0, 100);

                    AnimationHelper.AddFadeAnim(sb, ServerInfoLabel, 1, 1000);
                    AnimationHelper.AddFadeAnim(sb, VisitEmbySiteButton, 1, 1000);
                    AnimationHelper.AddVisibleAnimation(sb, VisitEmbySiteButton);
                    AnimationHelper.AddFadeAnim(sb, NextButton, 1, 500);
                    AnimationHelper.AddVisibleAnimation(sb, NextButton);
                    break;                
            }
            sb.Begin();
        }

    }
}
