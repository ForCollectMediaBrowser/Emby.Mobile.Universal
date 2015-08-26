using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public MainViewModel(IServices services) : base(services)
        {
        }

        public string ConnectedTo => string.Format(GetLocalizedString("LabelServerConnected"), Services.ServerInfo?.ServerInfo?.Name);

        public RelayCommand SignOutCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var signedInWithConnect = AuthenticationService.SignedInUsingConnect;
                    if (await AuthenticationService.SignOut())
                    {
                        if (signedInWithConnect)
                        {
                            Services.NavigationService.NavigateToEmbyConnect();
                        }
                        else
                        {
                            Services.NavigationService.NavigateToChooseProfile();
                        }

                        Services.NavigationService.ClearBackStack();
                    }
                });
            }
        }
    }
}
