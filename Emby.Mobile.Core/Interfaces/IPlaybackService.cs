using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IPlaybackService
    {
        event EventHandler<PlayStateChangedEventArgs> PlaystateChanged;
        event EventHandler<PlayerPositionEventArgs> PlayerPositionChanged;
        long? CurrentPositionTicks { get; }
        long? CurrentDurationTicks { get; }
        bool HasPlaylistItems { get; }
        bool HasCurrentItem { get; }
        bool HasUpcomingItem { get; }
        bool IsPlaying { get; }
        BaseItemDto CurrentItem { get; }
        BaseItemDto UpcomingItem { get; }
        List<PlaylistItem> Playlist { get; }
        List<IMediaPlayer> AvailablePlayers { get; }

        bool RegisterPlayer(IMediaPlayer player);
        void AddToPlaylist(BaseItemDto item);
        void AddToPlaylist(IList<BaseItemDto> items);
        void DecreaseVolume();
        void IncreaseVolume();
        Task Initialize();
        void Pause();
        Task<bool> PlayItem(BaseItemDto item, long position = 0);
        Task<bool> PlayItems(IList<BaseItemDto> items);
        Task<bool> PlayItems(string[] itemIds, long? position);
        void RemoveFromPlaylist(string itemId);
        void ResumeFromPause();
        Task<bool> Seek(long newPosition);
        Task<bool> SetAudioStreamIndex(int? index);
        Task<bool> SetNext();
        Task<bool> SetPrevious();
        Task<bool> SetSubtitleIndex(int? index);
        Task<bool> SetVolume(double volume);
        Task<bool> SkipToItem(string itemId);
        Task<bool> Stop();

        void ReportPlaybackStarted(PlaybackStartInfo info);
        void ReportPlaybackStopped(PlaybackStopInfo info);
        void ReportPlaybackProgress(PlaybackProgressInfo info);
    }
}
