namespace Emby.Mobile.Core.Playback
{
    public enum PlaylistState
    {
        /// <summary>
        /// Item is queued in playlist
        /// </summary>
        Queued = 0,
        /// <summary>
        /// Item is currently playing
        /// </summary>
        Playing = 1,
        /// <summary>
        /// Item has been played already
        /// </summary>
        Played = 2,
    }
}
