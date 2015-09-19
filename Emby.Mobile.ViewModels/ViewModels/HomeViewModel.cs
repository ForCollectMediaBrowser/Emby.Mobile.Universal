using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.LiveTv;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Querying;

namespace Emby.Mobile.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IServices services) : base(services)
        {
            ResumableItems = new ObservableCollection<ItemViewModel>();
            LatestVideoItems = new ObservableCollection<ItemViewModel>();
            LatestMusicItems = new ObservableCollection<ItemViewModel>();
            WhatsOnItems = new ObservableCollection<ItemViewModel>();
        }

        public override Task Refresh()
        {
            return LoadData();
        }

        public string ArtistBackdrop => GetAlbumArtistBackdrop();

        public bool HasLatestVideoItems => LatestVideoItems.Any();
        public bool HasLatestMusicItems => LatestMusicItems.Any();
        public bool HasResumableItems => ResumableItems.Any();
        public bool HasWhatsOnItems => WhatsOnItems.Any();

        public ObservableCollection<ItemViewModel> ResumableItems { get; set; }

        public ObservableCollection<ItemViewModel> LatestVideoItems { get; set; }

        public ObservableCollection<ItemViewModel> LatestMusicItems { get; set; }

        public ObservableCollection<ItemViewModel> WhatsOnItems { get; set; }

        private async Task LoadData()
        {
            string parentId = null;
            try
            {
                var response = await ApiClient.GetRootFolderAsync(AuthenticationService.SignedInUserId);
                if (response != null)
                {
                    parentId = response.Id;
                }
            }
            catch (HttpException e)
            { }

            var tasks = new List<Task>
            {
                LoadResumableItems(parentId),
                LoadLatestVideo(parentId),
                LoadLatestMusic(parentId),
                LoadWhatsOn()
            };
            await Task.WhenAll(tasks);
        }

        private async Task LoadResumableItems(string parentId)
        {
            try
            {
                var response = await ApiClient.GetItemsAsync(new ItemQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] { ItemSortBy.DatePlayed },
                    SortOrder = SortOrder.Descending,
                    Filters = new[] { ItemFilter.IsNotFolder, ItemFilter.IsResumable },
                    IncludeItemTypes = new[] { "Movie", "Episode" },
                    CollapseBoxSetItems = false,
                    Fields = new[] { ItemFields.SyncInfo, ItemFields.MediaSources, ItemFields.Taglines },

                    Limit = 6,
                    ParentId = parentId,
                    Recursive = true
                });
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    ResumableItems = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                }
            }
            catch (HttpException he)
            {

            }
        }

        private async Task LoadLatestVideo(string parentId)
        {
            try
            {
                var response = await ApiClient.GetLatestItems(new LatestItemsQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    GroupItems = true,
                    IsPlayed = false,
                    IncludeItemTypes = new[] { "Movie", "Episode" },
                    Fields = new[] { ItemFields.SyncInfo, ItemFields.MediaSources, ItemFields.Taglines },
                    EnableImageTypes = new[] { ImageType.Banner, ImageType.Backdrop, ImageType.Primary, ImageType.Thumb },
                    Limit = 10,
                    ParentId = parentId
                });
                if (!response.IsNullOrEmpty())
                {
                    LatestVideoItems = response.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                }
            }
            catch (HttpException he)
            {

            }
        }

        private async Task LoadLatestMusic(string parentId)
        {
            try
            {
                var response = await ApiClient.GetItemsAsync(new ItemQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] { ItemSortBy.DateCreated },
                    SortOrder = SortOrder.Descending,
                    Filters = new[] { ItemFilter.IsUnplayed },
                    IncludeItemTypes = new[] { "MusicAlbum" },
                    Fields = new[] { ItemFields.SyncInfo, ItemFields.MediaSources },
                    EnableImageTypes = new[] { ImageType.Banner, ImageType.Backdrop, ImageType.Primary, ImageType.Thumb },
                    Limit = 6,
                    Recursive = true,
                    ParentId = parentId

                });
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    LatestMusicItems = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                }
            }
            catch (HttpException he)
            {

            }
        }

        private async Task LoadWhatsOn()
        {
            if (AuthenticationService.SignedInUser.Policy.EnableLiveTvAccess)
            {
                try
                {
                    var response = await ApiClient.GetRecommendedLiveTvProgramsAsync(new RecommendedProgramQuery
                    {
                        UserId = AuthenticationService.SignedInUserId,
                        IsAiring = true,
                        Limit = 10
                    });

                    if (response != null && !response.Items.IsNullOrEmpty())
                    {
                        WhatsOnItems = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                    }
                }
                catch (HttpException he)
                {

                }
            }
        }

        private string GetAlbumArtistBackdrop()
        {
            var item = LatestMusicItems?.FirstOrDefault(x => !string.IsNullOrEmpty(x.ItemInfo.ParentBackdropItemId));
            if (item != null)
            {
                return item.ParentBackdropImageMedium;
            }
            //TODO Replace with other image
            return "ms-appx:///Assets/Backdrops/Splash_Screen_Landscape_1920x1080_nologo.png";
        }
    }
}
