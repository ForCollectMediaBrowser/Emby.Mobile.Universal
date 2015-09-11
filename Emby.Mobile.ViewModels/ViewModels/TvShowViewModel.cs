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
    public class TvShowViewModel : PageViewModelBase, IItemSettable
    {
        private bool _seasonsLoaded;

        public TvShowViewModel(IServices services) : base(services)
        {
        }

        public ItemViewModel Item { get; private set; }

        public ObservableCollection<ItemViewModel> Seasons { get; set; }

        public void SetItem(BaseItemDto item)
        {
            Item = new ItemViewModel(Services, item);
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
            if (_seasonsLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                SetProgressBar("**Loading seasons...");

                var query = new SeasonQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SeriesId = Item.ItemInfo.Id,
                    Fields = new[]
                    {
                        ItemFields.ParentId,
                        ItemFields.MediaSources,
                        ItemFields.SyncInfo
                    },
                    // TODO: When we have settings we need to sort this out
                    //IsMissing = App.SpecificSettings.ShowMissingEpisodes,
                    //IsVirtualUnaired = App.SpecificSettings.ShowUnairedEpisodes
                };

                Log.Info("Getting seasons for TV Show [{0}] ({1})", Item.Name, Item.ItemInfo.Id);

                var seasons = await ApiClient.GetSeasonsAsync(query);
                if (seasons != null && !seasons.Items.IsNullOrEmpty())
                {
                    Seasons = seasons.Items.OrderBy(x => x.IndexNumber).Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                    _seasonsLoaded = Seasons.Any();
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
