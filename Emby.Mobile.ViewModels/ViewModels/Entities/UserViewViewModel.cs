using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class UserViewViewModel : ItemViewModel
    {
        public UserViewViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {
        }
    }
}
