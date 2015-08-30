using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Emby.Mobile.ViewModels
{
    public class BurgerMenuViewModel : ViewModelBase
    {
        public BurgerMenuViewModel(IServices services) : base(services)
        {
        }

        public string Username { get; set; }
        public string ProfilePicture { get; set; }

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
    }
}
