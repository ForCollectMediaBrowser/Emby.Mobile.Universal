using System;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Views;
using Emby.Mobile.Universal.Views.Connect;

namespace Emby.Mobile.Universal.Services
{
    public class NavigationService : Cimbalino.Toolkit.Services.NavigationService, INavigationService
    {
        public void ClearBackStack()
        {
            while (CanGoBack)
            {
                RemoveBackEntry();
            }
        }

        public bool NavigateToEmbyConnect()
        {
            return Navigate<EmbyConnectView>();
        }

        public bool NavigateToServerSelection()
        {
            return Navigate<ChooseServerView>();
        }

        public bool NavigateToHome()
        {
            return Navigate<MainView>();
        }

        public bool NavigateToSignUp()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToConnectFirstRun()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToChooseProfile()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToFirstRun()
        {
            return false;
        }

        public bool NavigateToManualServerEntry()
        {
            return Navigate<ManualServerEntryView>();
        }
    }
}
