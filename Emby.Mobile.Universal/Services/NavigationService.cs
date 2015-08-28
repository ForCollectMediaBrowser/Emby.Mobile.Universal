﻿using System;
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

        public bool NavigateToEmbyConnectSignUp()
        {
            return Navigate<EmbyConnectSignUpView>();
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
            return Navigate<ChooseUserProfileView>();
        }

        public bool NavigateToFirstRun()
        {
            return Navigate<Views.FirstRun.WelcomeView>();
        }

        public bool NavigateToManualServerEntry()
        {
            return Navigate<ManualServerEntryView>();
        }

        public bool NavigateToManualLocalUserSignIn()
        {
            return Navigate<ManualLocalUserSignInView>();
        }
    }
}
