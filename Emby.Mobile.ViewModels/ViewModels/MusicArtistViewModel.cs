﻿using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public class MusicArtistViewModel : PageViewModelBase, IItemSettable
    {
        public MusicArtistViewModel(IServices services) : base(services)
        {
        }

        public ItemViewModel Item { get; private set; }
        public void SetItem(BaseItemDto item)
        {
            if (Item == null || Item.ItemInfo.Id != item.Id)
            {
                Item = new ItemViewModel(Services, item);
            }
        }
    }
}
