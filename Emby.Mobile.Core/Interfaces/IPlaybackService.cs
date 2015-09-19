using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IPlaybackService
    {
        event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;
        event EventHandler<PlayerPositionEventArgs> PlayerPositionChanged;
        event EventHandler<PlaybackInfoEventArgs> PlaybackInfoChanged;
        long? CurrentPositionTicks { get; }
        long? CurrentDurationTicks { get; }
        bool HasPlaylistItems { get; }
        bool HasCurrentItem { get; }
        bool HasUpcomingItem { get; }
        bool IsPlaying { get; }
        PlaylistItem CurrentItem { get; }
        PlaylistItem UpcomingItem { get; }
        List<PlaylistItem> Playlist { get; }
        List<IMediaPlayer> AvailablePlayers { get; }
        bool RegisterPlayer(IMediaPlayer player);
        void AddToPlaylist(BaseItemDto item);
        void AddToPlaylist(IList<BaseItemDto> items);
        void DecreaseVolume();
        void IncreaseVolume();
        void Pause();
        Task<bool> PlayItem(BaseItemDto item, long position = 0);
        Task<bool> PlayItems(IList<BaseItemDto> items, int? startingItem = null);
        Task<bool> PlayItems(string[] itemIds, long? position);
        void RemoveFromPlaylist(string itemId);
        void RemoveFromPlaylist(PlaylistItem item);
        void ResumeFromPause();
        Task<bool> Seek(long newPosition);
        void SetAudioStreamIndex(int? index);
        Task<bool> SetNext();
        Task<bool> SetPrevious();
        void SetSubtitleIndex(int? index);
        void SetVolume(double volume);
        void SkipToItem(string itemId);
        void Stop();

        void ReportPlaybackStarted(PlaybackStartInfo info);
        void ReportPlaybackStopped(PlaybackStopInfo info, StreamInfo streamInfo);
        void ReportPlaybackProgress(PlaybackProgressInfo info, StreamInfo streamInfo);
        void ReportPlaylistStatus(IList<PlaylistItem> playlist);
        void ReportPlayerState(PlayerState state);
    }
}
