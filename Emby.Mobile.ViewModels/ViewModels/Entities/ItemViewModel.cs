﻿using System.Diagnostics;
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
        public string Tagline => ItemInfo?.Taglines?.Count > 0 ? ItemInfo.Taglines[0] : "";

        public string PrimaryImageLarge => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimaryLarge) : "ms-appx:///Assets/Tiles/Square150x150.png";
        public string PrimaryImageMedium => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimaryMedium) : "ms-appx:///Assets/Tiles/Square150x150.png";
        public string PrimaryImageSmall => ItemInfo?.HasPrimaryImage ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemPrimarySmall) : "ms-appx:///Assets/Tiles/Square150x150.png";
        public string BackdropImage => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemBackdropMax) : "ms-appx:///Assets/Tiles/Square150x150.png";
        public string BackdropImageMedium => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemBackdropMedium) : "ms-appx:///Assets/Tiles/Square150x150.png";

        public string[] BackdropImages => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetBackdropImageUrls(ItemInfo, ImageOptionsHelper.ItemBackdropLarge) : new string[0];
        public string ThumbImage => ItemInfo?.HasThumb ?? false ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemThumbMedium) : BackdropImageMedium;
        public bool HasThumb => ItemInfo?.HasThumb ?? false;

        public bool CanStream => ItemInfo.CanStream(AuthenticationService.SignedInUser);

        public RelayCommand NavigateToItem
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Services.UiInteractions.NavigationService.NavigateToItem(ItemInfo);
                });
            }
        }

        public RelayCommand PlayItemCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //HACK Change this to the real deal.
                    Services.Playback.PlayItem(ItemInfo);
                });
            }
        }

        public async void LoadAllData()
        {
            //TODO Do we want to show progress here?
            ItemInfo = await ApiClient.GetItemAsync(ItemInfo.Id, AuthenticationService.SignedInUserId);
        }
    }
}
