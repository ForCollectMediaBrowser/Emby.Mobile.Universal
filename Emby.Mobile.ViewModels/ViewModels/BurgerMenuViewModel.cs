using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using Emby.Mobile.Universal.Core.Helpers;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels
{
    public class BurgerMenuViewModel : ViewModelBase
    {
        private bool _viewsLoaded;

        public BurgerMenuViewModel(IServices services) : base(services)
        {
            if (!IsInDesignMode)
            {
                AuthenticationService.UserChanged += (sender, args) =>
                {
                    Start();
                };

                Views = new ObservableCollection<ItemViewModel>();
            }
        }

        public bool BurgerIsVisible { get; set; }
        public bool CanChangeServer => AuthenticationService.SignedInUsingConnect;
        public UserDtoViewModel User { get; set; }
        public ObservableCollection<ItemViewModel> Views { get; set; }

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

        public RelayCommand ChangeServerCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ServerHelper.ChangeServer(Services);
                });
            }
        }

        public void Start()
        {
            SetUsernameAndProfilePicture();
            LoadViews(true).ConfigureAwait(false);

            RaisePropertyChanged(() => CanChangeServer);
        }

        public void ShowHide(bool show)
        {
            BurgerIsVisible = show;
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

        private async Task LoadViews(bool isRefresh)
        {
            if ((_viewsLoaded && !isRefresh) || !AuthenticationService.IsSignedIn)
            {
                return;
            }

            try
            {
                var item = await ApiClient.GetUserViews(AuthenticationService.SignedInUserId);
                if (item != null && !item.Items.IsNullOrEmpty())
                {
                    Views.Clear();
                    var views = item.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();

                    Views = views;

                    _viewsLoaded = true;
                }
            }
            catch (HttpException ex)
            {
                
            }
        }
    }
}
