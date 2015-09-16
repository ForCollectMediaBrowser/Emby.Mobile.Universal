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
            LatestVideo = new ObservableCollection<ItemViewModel>();
            LatestMusic = new ObservableCollection<ItemViewModel>();
            WhatsOn = new ObservableCollection<ItemViewModel>();
        }        

        public bool HasLatestVideo => LatestVideo.Any();
        public bool HasLatestMusic => LatestMusic.Any();
        public bool HasResumableItems => ResumableItems.Any();
        public bool HasWhatsOn => WhatsOn.Any();        

        public ObservableCollection<ItemViewModel> ResumableItems { get; set; }

        public ObservableCollection<ItemViewModel> LatestVideo { get; set; }

        public ObservableCollection<ItemViewModel> LatestMusic { get; set; }

        public ObservableCollection<ItemViewModel> WhatsOn { get; set; }

        private async Task LoadData()
        {            
            await LoadResumableItems();
            await LoadLatestVideo();
            await LoadLatestMusic();
            await LoadWhatsOn();           
        }

        private async Task LoadResumableItems()
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
                    EnableImageTypes = new[] { ImageType.Banner, ImageType.Backdrop, ImageType.Primary, ImageType.Thumb },
                    Limit = 6,
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

        private async Task LoadLatestVideo()
        {
            try
            {
                var response = await ApiClient.GetItemsAsync(new ItemQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] { ItemSortBy.DateLastContentAdded },
                    SortOrder = SortOrder.Descending,
                    Filters = new[] { ItemFilter.IsNotFolder, ItemFilter.IsUnplayed },
                    IncludeItemTypes = new[] { "Movie", "Episode" },
                    CollapseBoxSetItems = true,
                    Fields = new[] { ItemFields.SyncInfo, ItemFields.MediaSources, ItemFields.Taglines },
                    EnableImageTypes = new[] { ImageType.Banner, ImageType.Backdrop, ImageType.Primary, ImageType.Thumb },
                    Limit = 10,
                    Recursive = true
                });
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    LatestVideo = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                }
            }
            catch (HttpException he)
            {

            }
        }

        private async Task LoadLatestMusic()
        {
            try
            {
                var response = await ApiClient.GetItemsAsync(new ItemQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] { ItemSortBy.DateLastContentAdded },
                    SortOrder = SortOrder.Descending,
                    Filters = new[] { ItemFilter.IsNotFolder, ItemFilter.IsUnplayed },
                    IncludeItemTypes = new[] { "Album" },
                    CollapseBoxSetItems = true,
                    Fields = new[] { ItemFields.SyncInfo, ItemFields.MediaSources, ItemFields.Taglines },
                    EnableImageTypes = new[] { ImageType.Banner, ImageType.Backdrop, ImageType.Primary, ImageType.Thumb },
                    Limit = 10,
                    Recursive = true
                });
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    LatestMusic = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
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
                        WhatsOn = response.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                    }
                }
                catch (HttpException he)
                {

                }
            }
        }
    }
}
