using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Core.Playback;
using Emby.Mobile.Helpers;
using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Querying;

namespace Emby.Mobile.ViewModels
{
    public class PhotoAlbumViewModel : PageViewModelBase
    {
        private bool _photosLoaded;
        public PhotoAlbumViewModel(IServices services) : base(services)
        {
        }

        public List<PhotoViewModel> Photos { get; set; }

        public ItemViewModel PhotoAlbum { get; set; }

        public RelayCommand<PhotoViewModel> LaunchPhotoViewerCommand
        {
            get
            {
                return new RelayCommand<PhotoViewModel>(startingPhoto =>
                {
                    var items = Photos.Select(x => x.ItemInfo).ToList();
                    var startingItem = items.IndexOf(startingPhoto.ItemInfo);

                    Services.Playback.PlayItems(items, startingItem);
                });
            }
        }

        public override void OnNavigatedTo(NavigationMode mode, BaseItemDto item)
        {
            if (PhotoAlbum == null || item.Id != PhotoAlbum?.ItemInfo.Id)
            {
                Photos = new List<PhotoViewModel>();
                PhotoAlbum = new ItemViewModel(Services, item);
            }

            base.OnNavigatedTo(mode, item);
        }

        protected override Task PageLoaded()
        {
            return LoadData(false);
        }

        protected override Task Refresh()
        {
            return LoadData(true);
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_photosLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                SetProgressBar("**Getting photos...");

                var query = new ItemQuery
                {
                    ParentId = PhotoAlbum.ItemInfo.Id,
                    UserId = AuthenticationService.SignedInUserId,
                    SortBy = new[] {"DateCreated"},
                    SortOrder = SortOrder.Descending,
                    IncludeItemTypes = new[] {"Photo"},
                    Recursive = true,
                    EnableImageTypes = new[] {ImageType.Backdrop, ImageType.Primary, ImageType.Logo}
                };

                var response = await ApiClient.GetItemsAsync(query);
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    var items = response.Items.Select(x => new PhotoViewModel(Services, x)).ToList();

                    Photos = items;
                    _photosLoaded = Photos.Any();
                }
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
