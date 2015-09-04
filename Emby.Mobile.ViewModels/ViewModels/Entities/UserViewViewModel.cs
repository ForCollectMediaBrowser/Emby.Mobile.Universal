using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class UserViewViewModel : ItemViewModel
    {
        private bool _cardLoaded;

        public UserViewViewModel(IServices services, BaseItemDto itemInfo) : base(services, itemInfo)
        {

        }

        private async Task GetCardData(bool isRefresh)
        {
            if (_cardLoaded && !isRefresh)
            {
                return;
            }
        }
    }
}
