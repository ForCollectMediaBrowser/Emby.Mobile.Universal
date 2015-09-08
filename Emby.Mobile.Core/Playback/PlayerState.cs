namespace Emby.Mobile.Core.Playback
{
    public enum PlayerState
    {
        /// <summary>
        /// Players current state is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Player is stopped
        /// </summary>
        Stopped = 1,
        /// <summary>
        /// Player is playing
        /// </summary>
        Playing = 2,
        /// <summary>
        /// Player is paused
        /// </summary>
        Paused = 3,
    }
}
