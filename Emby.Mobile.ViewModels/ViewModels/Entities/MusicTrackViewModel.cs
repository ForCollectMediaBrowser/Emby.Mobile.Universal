using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class MusicTrackViewModel : ItemViewModel
    {
        public MusicTrackViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {
        }
    }
}
