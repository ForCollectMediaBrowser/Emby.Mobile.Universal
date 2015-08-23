namespace Emby.Mobile.Core.Interfaces
{
    public interface INavigationService : Cimbalino.Toolkit.Services.INavigationService
    {
        bool NavigateToEmbyConnect();
        bool NavigateToServerSelection();
        bool NavigateToHome();
        bool NavigateToSignUp();
        bool NavigateToConnectFirstRun();
        bool NavigateToChooseProfile();
        bool NavigateToFirstRun();
    }
}
