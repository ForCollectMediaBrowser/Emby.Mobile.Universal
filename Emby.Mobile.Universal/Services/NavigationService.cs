using System;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Views;
using Emby.Mobile.Universal.Views.Connect;
using Emby.Mobile.Universal.Views.FirstRun;
using Emby.Mobile.Universal.Views.UserViews;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Services
{
    public class NavigationService : Cimbalino.Toolkit.Services.NavigationService, INavigationService
    {
        public void ClearBackStack()
        {
            while (CanGoBack)
            {
                RemoveBackEntry();
            }
        }

        public bool NavigateToEmbyConnect()
        {
            return Navigate<ConnectView>();
        }

        public bool NavigateToEmbyConnectSignUp()
        {
            return Navigate<ConnectSignUpView>();
        }

        public bool NavigateToServerSelection()
        {
            return Navigate<ChooseServerView>();
        }

        public bool NavigateToLocalServerSelection()
        {
            return Navigate<ChooseLocalServerView>();
        }

        public bool NavigateToHome()
        {
            return Navigate<MainView>();
        }

        public bool NavigateToSignUp()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToConnectFirstRun()
        {
            throw new NotImplementedException();
        }

        public bool NavigateToChooseProfile()
        {
            return Navigate<ChooseUserProfileView>();
        }

        public bool NavigateToFirstRun()
        {
            return Navigate<WelcomeView>();
        }

        public bool NavigateToManualServerEntry()
        {
            return Navigate<ManualServerEntryView>();
        }

        public bool NavigateToManualLocalUserSignIn()
        {
            return Navigate<ManualLocalUserSignInView>();
        }

        public bool NavigateToPinLogin()
        {
            return Navigate<ConnectPinEntryView>();
        }

        public bool NavigateToSettings()
        {
            return Navigate<SettingsView>();
        }

        public bool NavigateToPreferences()
        {
            return false;
        }

        public bool NavigateToItem(BaseItemDto item)
        {
            bool value;

            var type = item.Type.ToLower();
            if (type.Contains("collectionfolder")) type = "collectionfolder";
            if (type.StartsWith("genre")) type = "genre";
            switch (type)
            {
                case "collectionfolder":
                case "genre":
                case "trailercollectionfolder":
                case "playlistsfolder":
                case "userview":
                    value = HandleCollectionNavigation(item);
                    break;
                //    case "photoalbum":
                //    case "folder":
                //    case "boxset":
                //        break;
                case "movie":
                    value = Navigate<MovieView>(item);
                    break;
                case "series":
                    value = Navigate<TvShowView>(item);
                    break;
                case "season":
                    value = Navigate<SeasonView>(item);
                    break;
                case "episode":
                    value = Navigate<EpisodeView>(item);
                    break;
                //    case "trailer":
                //        break;
                //    case "musicartist":
                //    case "artist":
                //        break;
                //    case "musicalbum":
                //        break;
                //    case "channel":
                //    case "channelfolderitem":
                //        break;
                //    case "playlist":
                //        break;
                //    case "person":
                //        var actor = new BaseItemPerson
                //        {
                //            Name = item.Name,
                //            Id = item.Id,
                //            PrimaryImageTag = item.HasPrimaryImage ? item.ImageTags.FirstOrDefault(x => x.Key == ImageType.Primary).Value : string.Empty
                //        };

                //        break;
                default:
                    value = Navigate<GenericItemView>(item);
                    break;
            }

            return value;
        }

        private bool HandleCollectionNavigation(BaseItemDto item)
        {
            var value = false;
            var viewType = string.IsNullOrEmpty(item.CollectionType) ? string.Empty : item.CollectionType.ToLower();
            switch (viewType)
            {
                case "movies":
                    value = Navigate<MovieUserView>(item);
                    break;
                case "tvshows":
                    value = Navigate<TvUserView>(item);
                    break;
                case "music":
                    value = Navigate<MusicUserView>(item);
                    break;
                case "channels":
                    break;
                case "livetv":
                    break;
                case "playlists":
                    value = Navigate<PlaylistUserView>(item);
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
