using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public class SeasonViewModel : PageViewModelBase, IItemSettable
    {
        public SeasonViewModel(IServices services) : base(services)
        {
        }

        public ObservableCollection<ItemViewModel> Episodes { get; set; }
        public ItemViewModel SelectedEpisode { get; set; }
        
        public ItemViewModel Item { get; private set; }
        public void SetItem(BaseItemDto item)
        {
            Item = new ItemViewModel(Services, item);
        }

        public async Task SetSelectedEpisode(BaseItemDto item)
        {
            if (Episodes.IsNullOrEmpty())
            {
                // TODO: Get Episodes
            }

            var episode = Episodes.FirstOrDefault(x => x.ItemInfo.Id == item.Id);
            SelectedEpisode = episode;
        }
    }
}
