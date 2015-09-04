namespace Emby.Mobile.Core.Interfaces
{
    public interface INavigationService : Cimbalino.Toolkit.Services.INavigationService
    {
        void ClearBackStack();
        bool NavigateToEmbyConnect();
        bool NavigateToEmbyConnectSignUp();
        bool NavigateToServerSelection();
        bool NavigateToHome();
        bool NavigateToSignUp();
        bool NavigateToConnectFirstRun();
        bool NavigateToChooseProfile();
        bool NavigateToFirstRun();
        bool NavigateToLocalServerSelection();
        bool NavigateToManualServerEntry();
        bool NavigateToManualLocalUserSignIn();
        bool NavigateToPinLogin();
        bool NavigateToSettings();
        bool NavigateToPreferences();
    }
}
