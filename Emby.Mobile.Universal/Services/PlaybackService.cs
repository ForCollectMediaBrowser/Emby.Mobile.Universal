using Emby.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Session;

namespace Emby.Mobile.Universal.Services
{
    internal class PlaybackService : IPlaybackService
    {
        public List<IMediaPlayer> AvailablePlayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long? CurrentDurationTicks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BaseItemDto CurrentItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long? CurrentPositionTicks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasCurrentItem => CurrentItem != null;

        public bool HasPlaylistItems => Playlist?.Any() == true;

        public bool HasUpcomingItem => UpcomingItem != null;

        public bool IsPlaying
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<PlaylistItem> Playlist
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BaseItemDto UpcomingItem
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<PlayerPositionEventArgs> PlayerPositionChanged;
        public event EventHandler<PlayStateChangedEventArgs> PlaystateChanged;

        public void AddToPlaylist(IList<BaseItemDto> items)
        {
            throw new NotImplementedException();
        }

        public void AddToPlaylist(BaseItemDto item)
        {
            throw new NotImplementedException();
        }

        public void DecreaseVolume()
        {
            throw new NotImplementedException();
        }

        public void IncreaseVolume()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PlayItem(BaseItemDto item, long position = 0)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PlayItems(IList<BaseItemDto> items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PlayItems(string[] itemIds, long? position)
        {
            throw new NotImplementedException();
        }

        public bool RegisterPlayer(IMediaPlayer player)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromPlaylist(string itemId)
        {
            throw new NotImplementedException();
        }

        public void ReportPlaybackProgress(PlaybackProgressInfo info)
        {
            throw new NotImplementedException();
        }

        public void ReportPlaybackStarted(PlaybackStartInfo info)
        {
            throw new NotImplementedException();
        }

        public void ReportPlaybackStopped(PlaybackStopInfo info)
        {
            throw new NotImplementedException();
        }

        public void ResumeFromPause()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Seek(long newPosition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAudioStreamIndex(int? index)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetNext()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPrevious()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetSubtitleIndex(int? index)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetVolume(double volume)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SkipToItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Stop()
        {
            throw new NotImplementedException();
        }
    }
}
