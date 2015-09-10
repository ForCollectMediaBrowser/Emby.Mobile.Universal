using System.Collections.ObjectModel;
using System.Text;
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
        public HeaderMenuViewModel(IServices services) : base(services)
        {
            if (!IsInDesignMode)
            {
                AuthenticationService.UserChanged += (sender, args) =>
                {
                    Start();
                };

                Services.ServerInfo.ServerInfoChanged += (sender, info) =>
                {
                    RaisePropertyChanged(() => ConnectedToServerAddress);
                    RaisePropertyChanged(() => ConnectedToServerName);
                };

                SearchResults = new ObservableCollection<SearchHint>();
            }
        }

        public bool IsVisible { get; set; } = true;
        public string SearchText { get; set; }
        public bool CanChangeServer => AuthenticationService.SignedInUsingConnect;
        public string ConnectedToServerName => Services.ServerInfo?.ServerInfo?.Name;
        public string ConnectedToServerAddress => CreateToolTip();

        private string CreateToolTip()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Services.ServerInfo?.ServerInfo?.ManualAddress))
            {
                sb.AppendLine($"{Resources.LabelManualAddress}: {Services.ServerInfo?.ServerInfo?.ManualAddress}");
            }
            else
            {
                if (!string.IsNullOrEmpty(Services.ServerInfo?.ServerInfo?.LocalAddress))
                {
                    sb.AppendLine($"{Resources.LabelLocalAddress}: {Services.ServerInfo?.ServerInfo?.LocalAddress}");
                }
                if (!string.IsNullOrEmpty(Services.ServerInfo?.ServerInfo?.RemoteAddress))
                {
                    sb.AppendLine($"{Resources.LabelRemoteAddress}: {Services.ServerInfo?.ServerInfo?.RemoteAddress}");
                }
            }

            return sb.ToString();
        }

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

        public RelayCommand NavigateTopPreferencesCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToPreferences();
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

        public RelayCommand GoHomeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToHome();
                    Services.NavigationService.ClearBackStack();
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

        private async void OnSearchTextChanged()
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
