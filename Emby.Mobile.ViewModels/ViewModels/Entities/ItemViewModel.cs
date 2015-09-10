using System.Diagnostics;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels.Entities
{
    [DebuggerDisplay("Name: {Name}, Type: {Type}")]
    public class ItemViewModel : ViewModelBase
    {
        public ItemViewModel(IServices services, BaseItemDto itemInfo) : base(services)
        {
            ItemInfo = itemInfo;
        }

        public BaseItemDto ItemInfo { get; set; }

        public string Name => ItemInfo?.Name;
        public string MaterialIcon => ItemInfo.GetMaterialIcon();
        public string Type => ItemInfo?.Type;
        public string PrimaryImageLarge => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimaryLarge): "ms-appx:///Assets/Tiles/150x150Logo.png";
        public string PrimaryImageMedium => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimaryMedium) : "ms-appx:///Assets/Tiles/150x150Logo.png";
        public string PrimaryImageSmall => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimarySmall) : "ms-appx:///Assets/Tiles/150x150Logo.png";
        public string BackdropImage => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemBackdrop) : "ms-appx:///Assets/Tiles/150x150Logo.png";

        public RelayCommand NavigateToItem
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.NavigationService.NavigateToItem(ItemInfo);
                });
            }
        }

        public RelayCommand PlayItemCommand
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
