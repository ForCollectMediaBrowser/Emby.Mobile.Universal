namespace Emby.Mobile.Universal.BackgroundAudio
{
    public enum BackgroundTaskState
    {
        /// <summary>
        /// State of background audio task is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Background Audio task is started
        /// </summary>
        Started = 1,
        /// <summary>
        /// Background Audio task is running
        /// </summary>
        Running = 2,
        /// <summary>
        /// Background Audio task is Canceled
        /// </summary>
        Canceled = 3
    }
}
