using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Helpers;
using Emby.Mobile.ViewModels.Entities;
using Emby.Mobile.ViewModels.UserViews;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public abstract class UserViewModelBase : PageViewModelBase, IUserViewViewModel
    {
        protected UserViewModelBase(IServices services) : base(services)
        {
        }

        public ItemViewModel UserView { get; set; }

        public override void OnNavigatedTo(NavigationMode mode, BaseItemDto item)
        {
            if (UserView == null || item.Id != UserView?.ItemInfo.Id)
            {
                UserView = new ItemViewModel(Services, item);
            }

            base.OnNavigatedTo(mode, item);
        }
    }
}