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
        public string BackdropImage =>  ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemBackdropMax) ?? "ms-appx:///Assets/Tiles/Square150x150.png";
        public string BackdropImageMedium => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetImageUrl(ItemInfo.Id, ImageOptionsHelper.ItemBackdropMedium) : ParentBackdropImageMedium;
        public string[] BackdropImages => ItemInfo?.BackdropCount > 0 ? ApiClient?.GetBackdropImageUrls(ItemInfo, ImageOptionsHelper.ItemBackdropLarge) : new string[0];
        public string ParentBackdropImageMedium => !string.IsNullOrEmpty(ItemInfo?.ParentBackdropItemId) ? ApiClient?.GetImageUrl(ItemInfo.ParentBackdropItemId, ImageOptionsHelper.ItemBackdropMedium) : "ms-appx:///Assets/Tiles/150x150Logo.png";
        public string ThumbImage => HasThumb ? ApiClient?.GetImageUrl(GetThumbId(), ImageOptionsHelper.ItemThumbMedium) : BackdropImageMedium;

        public bool HasThumb => !string.IsNullOrEmpty(GetThumbId());
        public bool HasBackdrop => ItemInfo?.BackdropCount > 0;
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

        private string GetThumbId()
        {
            if (ItemInfo?.HasThumb == true)
            {
                return ItemInfo.Id;
            }
            if (!string.IsNullOrEmpty(ItemInfo?.SeriesId) && !string.IsNullOrEmpty(ItemInfo?.SeriesThumbImageTag))
            {
                return ItemInfo.SeriesId;
            }
            if (!string.IsNullOrEmpty(ItemInfo?.ParentThumbItemId))
            {
                return ItemInfo?.ParentThumbItemId;
            }

            return null;
        }
    }
}
