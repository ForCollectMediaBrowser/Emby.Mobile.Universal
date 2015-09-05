using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Core.Playback
{
    public class PlaylistItem
    {
        BaseItemDto Item { get; set; }
        PlaylistState State { get; set; }
    }
}
