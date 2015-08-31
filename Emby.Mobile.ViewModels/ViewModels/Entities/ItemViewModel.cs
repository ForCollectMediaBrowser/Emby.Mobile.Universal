using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    public class ItemViewModel : ViewModelBase
    {
        public ItemViewModel(IServices services, BaseItemDto itemInfo) : base(services)
        {
            ItemInfo = itemInfo;
        }

        public string Name => ItemInfo?.Name;

        public string MaterialIcon => ItemInfo.GetMaterialIcon();

        public BaseItemDto ItemInfo { get; set; }

        public RelayCommand NavigateToItem
        {
            get
            {
                return new RelayCommand(() =>
                {
                    
                });
            }
        }
    }
}
