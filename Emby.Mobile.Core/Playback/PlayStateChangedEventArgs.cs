using System;

namespace Emby.Mobile.Core.Playback
{
    public class PlayStateChangedEventArgs : EventArgs
    {
        PlayerState NewState { get; }
        PlayerState OldState { get; }

        public PlayStateChangedEventArgs(PlayerState newState) : this(newState, PlayerState.Unknown) { }
        public PlayStateChangedEventArgs(PlayerState newState, PlayerState oldState)
        {
            NewState = newState;
            OldState = oldState;
        }
    }
}
