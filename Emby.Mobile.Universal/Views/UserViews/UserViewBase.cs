using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Controls;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.ViewModels.UserViews;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views.UserViews
{
    public class UserViewBase : EmbyPage, ICanHasHeaderMenu
    {
        protected IUserViewViewModel View => DataContext as IUserViewViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            View?.OnNavigatedTo(e.NavigationMode.ExchangeMode(), item);

            base.OnNavigatedTo(e);
        }
    }
}
