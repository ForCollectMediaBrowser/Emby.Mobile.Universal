using System;
using System.Collections.Generic;
using Cimbalino.Toolkit.Services;
using INavigationService = Emby.Mobile.Core.Interfaces.INavigationService;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullNavigationService : INavigationService
    {
        #region Cimbalino stuff
        public event EventHandler Navigated;
        public event EventHandler<NavigationServiceBackKeyPressedEventArgs> BackKeyPressed;
        public Uri CurrentSource { get; } = null;
        public IEnumerable<KeyValuePair<string, string>> QueryString { get; } = null;
        public object CurrentParameter { get; } = null;
        public bool Navigate(string source)
        {
            return false;
        }

        public bool Navigate(Uri source)
        {
            return false;
        }

        public bool Navigate<T>()
        {
            return false;
        }

        public bool Navigate<T>(object parameter)
        {
            return false;
        }

        public bool Navigate(Type type)
        {
            return false;
        }

        public bool Navigate(Type type, object parameter)
        {
            return false;
        }

        public bool CanGoBack { get; } = false;
        public void GoBack()
        {
        }

        public bool CanGoForward { get; } = false;
        public void GoForward()
        {
        }

        public bool RemoveBackEntry()
        {
            return false;
        }
        #endregion

        public void ClearBackStack()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToEmbyConnect()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToServerSelection()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToHome()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool NavigateToManualServerEntry()
        {
            throw new NotImplementedException();
        }
    }
}
