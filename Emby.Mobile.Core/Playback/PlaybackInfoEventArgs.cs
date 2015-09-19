using System;

namespace Emby.Mobile.Core.Playback
{
    public class PlaybackInfoEventArgs : EventArgs
    {
        public string ItemId { get; set; }
        public long? PlaybackTicks { get; set; }

        public PlaybackInfoEventArgs(string itemId, long? playbackTicks)
        {
            ItemId = itemId;
            PlaybackTicks = playbackTicks;
        }
    }
}