using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.Universal.Views;
using Emby.Mobile.Universal.Views.Connect;
using Emby.Mobile.Universal.Views.FirstRun;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyApplicationFrame : Frame
    {
        private readonly List<Type> _noBurgersForYou = new List<Type>
        {
            typeof(WelcomeView),
            typeof(ChooseServerView),
            typeof(ChooseUserProfileView),
            typeof(ConnectPinEntryView),
            typeof(ConnectSignUpView),
            typeof(ConnectView),
            typeof(ManualServerEntryView),
            typeof(ManualLocalUserSignInView),
            typeof(StartupView),
            typeof(ChooseLocalServerView)
        }; 

        public EmbyApplicationFrame()
        {
            Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var pageType = e.Content.GetType();
            var show = !_noBurgersForYou.Contains(pageType);

            ViewModelLocator.Get<BurgerMenuViewModel>().ShowHide(show);
        }
    }
}
