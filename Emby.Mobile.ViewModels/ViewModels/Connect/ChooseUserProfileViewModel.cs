using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Strings;
using Emby.Mobile.Helpers;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Net;

namespace Emby.Mobile.ViewModels.Connect
{
    public class ChooseUserProfileViewModel : PageViewModelBase
    {
        private bool _profilesLoaded;

        public ChooseUserProfileViewModel(IServices services) : base(services)
        {
            if (IsInDesignMode)
            {
                UserProfiles = new ObservableCollection<UserDtoViewModel>
                {
                    new UserDtoViewModel(services, new UserDto
                    {
                        Name = "7illusions",
                        HasPassword = false,
                        LastActivityDate= System.DateTime.Now.AddMinutes(-30)
                    }),
                    new UserDtoViewModel(services, new UserDto
                    {
                        Name = "Scott",
                        HasPassword = true,
                        LastActivityDate= System.DateTime.Now.AddMinutes(-10)
                    })
                };
            }
        }

        protected override bool UseSystemForProgress { get; } = true;

        public ObservableCollection<UserDtoViewModel> UserProfiles { get; set; }

        public RelayCommand ManualUserProfileEntryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.UiInteractions.NavigationService.NavigateToManualServerEntry();
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

        

        protected override Task OnSignOut()
        {
            if (!UserProfiles.IsNullOrEmpty())
            {
                UserProfiles = null;
                _profilesLoaded = false;
            }

            return base.OnSignOut();
        }

        protected override Task PageLoaded()
        {
            return LoadData(false);
        }

        public override Task Refresh()
        {
            return LoadData(true);
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_profilesLoaded && !isRefresh)
            {
                return;
            }

            SetProgressBar(Resources.SysTrayGettingProfiles);

            try
            {
                var userProfiles = await ApiClient.GetPublicUsersAsync();

                if (userProfiles.IsNullOrEmpty())
                {
                    return;
                }
                
            
                UserProfiles = userProfiles.Select(x => new UserDtoViewModel(Services, x)).ToObservableCollection();

                _profilesLoaded = !UserProfiles.IsNullOrEmpty();
            }
            catch (HttpException ex)
            {
                Log.ErrorException("LoadData", ex);
            }
            finally
            {
                SetProgressBar();
            }
        }
    }
}
