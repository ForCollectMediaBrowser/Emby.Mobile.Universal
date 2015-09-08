using MediaBrowser.Model.Dto;
using System;

namespace Emby.Mobile.Core.Playback
{
    public class PlaylistItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public BaseItemDto Item { get; set; }
        public PlaylistState State { get; set; }

        public PlaylistItem(BaseItemDto item)
        {
            Item = item;
            State = PlaylistState.Queued;
        }
    }
}
