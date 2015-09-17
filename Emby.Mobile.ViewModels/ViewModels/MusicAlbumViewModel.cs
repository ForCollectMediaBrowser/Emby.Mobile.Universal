using System.Collections.ObjectModel;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public class MusicAlbumViewModel : PageViewModelBase, IItemSettable
    {
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
    }
}
