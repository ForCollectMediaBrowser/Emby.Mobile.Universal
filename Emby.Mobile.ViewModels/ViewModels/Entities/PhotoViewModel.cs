using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class PhotoViewModel : ItemViewModel
    {
        public PhotoViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {
        }
    }
}
