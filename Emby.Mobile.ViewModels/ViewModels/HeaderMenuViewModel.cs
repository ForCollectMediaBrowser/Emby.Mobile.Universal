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
using MediaBrowser.Model.Search;
using Emby.Mobile.Core.Strings;

namespace Emby.Mobile.ViewModels
{
    public class HeaderMenuViewModel : ViewModelBase
    {
        private bool _viewsLoaded;

        public HeaderMenuViewModel(IServices services) : base(services)
        {
            if (!IsInDesignMode)
            {
                AuthenticationService.UserChanged += (sender, args) =>
                {
                    Start();
                };

                SearchResults = new ObservableCollection<SearchHint>();
            }
        }

        public bool IsVisible { get; set; }
        public string SearchText { get; set; }
        public bool CanChangeServer => AuthenticationService.SignedInUsingConnect;
        public UserDtoViewModel User { get; set; }
        public ObservableCollection<SearchHint> SearchResults { get; set; }
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
            RaisePropertyChanged(() => CanChangeServer);
        }

        public void ShowHide(bool show)
        {
            IsVisible = show;
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

        async void OnSearchTextChanged()
        {
            var query = new SearchQuery
            {
                UserId = AuthenticationService.SignedInUserId,
                Limit = 20,
                StartIndex = 0,
                IncludeArtists = true,
                IncludeGenres = true,
                IncludeMedia = true,
                IncludePeople = true,
                IncludeStudios = true,
                SearchTerm = SearchText
            };

            try
            {
                SearchResults.Clear();
                var search = await ApiClient.GetSearchHintsAsync(query);
                if (search != null && !search.SearchHints.IsNullOrEmpty())
                {
                    SearchResults = search.SearchHints.ToObservableCollection();
                    SetProgressBar(Resources.SysTraySearching);
                }
            }
            catch (HttpException e)
            {

            }
            finally
            {
                SetProgressBar();
            }
        }
    }
}
