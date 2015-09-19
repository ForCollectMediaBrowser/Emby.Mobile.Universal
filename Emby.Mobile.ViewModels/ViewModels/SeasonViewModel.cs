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
    public class SeasonViewModel : PageViewModelBase, IItemSettable
    {
        private bool _episodesLoaded;

        public SeasonViewModel(IServices services) : base(services)
        {
        }

        public ObservableCollection<ItemViewModel> Episodes { get; set; }
        public ItemViewModel SelectedEpisode { get; set; }
        
        public ItemViewModel Item { get; private set; }
        public void SetItem(BaseItemDto item)
        {
            Item = new ItemViewModel(Services, item);
            _episodesLoaded = false;
        }

        public async Task SetSelectedEpisode(BaseItemDto item)
        {
            if (Episodes.IsNullOrEmpty())
            {
                await LoadData(false);
            }

            var episode = Episodes.FirstOrDefault(x => x.ItemInfo.Id == item.Id);
            SelectedEpisode = episode;
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
            if (_episodesLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                SetProgressBar("**Getting episodes...");

                var query = new EpisodeQuery
                {
                    UserId = AuthenticationService.SignedInUserId,
                    SeasonId = Item.ItemInfo.Id,
                    SeriesId = Item.ItemInfo.SeriesId,
                    Fields = new[]
                    {
                        ItemFields.ParentId,
                        ItemFields.Overview,
                        ItemFields.MediaSources,
                    },
                    // TODO When we have app-wide settings
                    //IsMissing = App.SpecificSettings.ShowMissingEpisodes,
                    //IsVirtualUnaired = App.SpecificSettings.ShowUnairedEpisodes
                };

                Log.Info("Getting episodes for Season [{0}] ({1}) of TV Show [{2}] ({3})", Item.ItemInfo.Name, Item.ItemInfo.Id, Item.ItemInfo.SeasonName, Item.ItemInfo.SeasonId);

                var episodes = await ApiClient.GetEpisodesAsync(query);
                if (episodes != null && !episodes.Items.IsNullOrEmpty())
                {
                    Episodes = episodes.Items.Select(x => new ItemViewModel(Services, x)).ToObservableCollection();
                    _episodesLoaded = Episodes.Any();
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
