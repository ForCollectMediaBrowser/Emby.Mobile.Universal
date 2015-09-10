using System.ComponentModel;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public interface IItemSettable : INotifyPropertyChanged
    {
        ItemViewModel Item { get; }
        void SetItem(BaseItemDto item);
    }
}