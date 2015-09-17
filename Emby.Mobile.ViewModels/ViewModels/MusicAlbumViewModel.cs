using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Net;
using MediaBrowser.Model.Querying;

namespace Emby.Mobile.ViewModels
{
    public class MusicAlbumViewModel : PageViewModelBase, IItemSettable
    {
        private bool _tracksLoaded;

        public MusicAlbumViewModel(IServices services) : base(services)
        {
        }

        public ObservableCollection<MusicTrackViewModel> Tracks { get; set; }

        public ItemViewModel Item { get; private set; }
        public void SetItem(BaseItemDto item)
        {
            if (Item == null || Item.ItemInfo.Id != item.Id)
            {
                Item = new ItemViewModel(Services, item);
            }
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_tracksLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                await RefreshAlbum();

                var query = new ItemQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    ParentId = Item.ItemInfo.Id,
                    IncludeItemTypes = new[] {"Audio"},
                    Fields = new[] {ItemFields.ParentId, ItemFields.MediaSources, ItemFields.SyncInfo},
                    Recursive = true
                };

                var response = await ApiClient.GetItemsAsync(query);
                if (response != null && !response.Items.IsNullOrEmpty())
                {
                    var items = response.Items.Select(x => new MusicTrackViewModel(Services, x)).ToObservableCollection();
                    Tracks = items;

                    _tracksLoaded = Tracks.Any();
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

        private async Task RefreshAlbum()
        {
            if (Item?.ItemInfo == null)
            {
                return;
            }

            try
            {
                var album = await ApiClient.GetItemAsync(Item.ItemInfo.Id, AuthenticationService.SignedInUserId);
                if (album != null)
                {
                    Item = new ItemViewModel(Services, album);
                }
            }
            catch (HttpException ex)
            {
                
            }
        }
    }
}
