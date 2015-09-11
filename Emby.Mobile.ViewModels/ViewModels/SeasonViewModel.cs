﻿using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.ViewModels.Entities;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.ViewModels
{
    public class SeasonViewModel : PageViewModelBase, IItemSettable
    {
        public SeasonViewModel(IServices services) : base(services)
        {
        }
        
        public ItemViewModel Item { get; private set; }
        public void SetItem(BaseItemDto item)
        {
            Item = new ItemViewModel(Services, item);
        }
    }
}
