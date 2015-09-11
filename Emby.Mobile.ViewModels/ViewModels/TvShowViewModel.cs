using System.Collections.ObjectModel;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public class TvShowViewModel : PageViewModelBase, IItemSettable
    {
        public TvShowViewModel(IServices services) : base(services)
        {
        }

        public ItemViewModel Item { get; private set; }

        public ObservableCollection<ItemViewModel> Seasons { get; set; }

        public void SetItem(BaseItemDto item)
        {
            Item = new ItemViewModel(Services, item);
        }
    }
}
