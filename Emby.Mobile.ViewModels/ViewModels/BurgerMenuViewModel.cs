using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class BurgerMenuViewModel : ViewModelBase
    {
        public BurgerMenuViewModel(IServices services) : base(services)
        {
            if (!IsInDesignMode)
            {
                AuthenticationService.UserChanged += (sender, args) =>
                {
                    SetUsernameAndProfilePicture();
                };
            }
        }

        public void Start()
        {
            SetUsernameAndProfilePicture();
        }

        private void SetUsernameAndProfilePicture()
        {
            Services.Dispatcher.RunAsync(() =>
            {
                if (!AuthenticationService.SignedInUsingConnect && !AuthenticationService.IsSignedIn)
                {
                    User = null;
                }
                else
                {
                    User = new UserDtoViewModel(Services, AuthenticationService.SignedInUser);
                }
            });
        }

        public UserDtoViewModel User { get; set; }

        public RelayCommand NavigateToSettingsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToSettings();
                });
            }
        }

        public RelayCommand SignOutCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await SignOutHelper.SignOut(Services);
                });
            }
        }
    }
}
