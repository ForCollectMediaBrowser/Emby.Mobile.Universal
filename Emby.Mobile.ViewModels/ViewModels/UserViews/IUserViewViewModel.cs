using System.ComponentModel;
using Emby.Mobile.Helpers;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.UserViews
{
    public interface IUserViewViewModel : INotifyPropertyChanged
    {
        ItemViewModel UserView { get; set; }
        void OnNavigatedTo(NavigationMode mode, BaseItemDto item);
    }
}
