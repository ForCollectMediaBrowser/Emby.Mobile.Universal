using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Dto;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.ViewModels
{
    public class ChooseUserProfileViewModel : PageViewModelBase
    {
        private bool _profilesLoaded;

        public ObservableCollection<UserDtoViewModel> UserProfiles { get; set; }
        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadData(true);
                });
            }
        }

        public RelayCommand ManualUserProfileEntryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToManualServerEntry();
                });
            }
        }

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

        protected override async Task PageLoaded()
        {
            await LoadData(false);
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

            }
            finally
            {
                SetProgressBar();
            }
        }
    }
}
