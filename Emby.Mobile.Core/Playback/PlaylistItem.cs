using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Core.Playback
{
    public class PlaylistItem
    {
        public BaseItemDto Item { get; set; }
        public PlaylistState State { get; set; }

        public PlaylistItem(BaseItemDto item)
        {
            Item = item;
            State = PlaylistState.Queued;
        }
    }
}
