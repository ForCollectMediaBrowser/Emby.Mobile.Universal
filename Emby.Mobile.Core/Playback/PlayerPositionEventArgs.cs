using System;

namespace Emby.Mobile.Core.Playback
{
    public class PlayerPositionEventArgs : EventArgs
    {
        public TimeSpan Position { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public double PlaybackSpeed { get; set; }
    }
}
