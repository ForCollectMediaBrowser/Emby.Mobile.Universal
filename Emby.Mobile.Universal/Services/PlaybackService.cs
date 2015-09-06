using Emby.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Session;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Services
{
    internal class PlaybackService : IPlaybackService
    {
        private IConnectionManager _connectionManager;
        private IServerInfoService _serverInfo;
        private IMediaPlayer _currentPlayer;
        private IApiClient _apiClient => _connectionManager?.GetApiClient(_serverInfo?.ServerInfo?.Id);

        public List<IMediaPlayer> AvailablePlayers { get; } = new List<IMediaPlayer>();

        public long? CurrentDurationTicks { get; }

        public PlaylistItem CurrentItem => Playlist?.FirstOrDefault(p => p.State == PlaylistState.Playing);


        public long? CurrentPositionTicks { get; }

        public bool HasCurrentItem => CurrentItem != null;

        public bool HasPlaylistItems => Playlist?.Any() == true;

        public bool HasUpcomingItem => UpcomingItem != null;

        public bool IsPlaying => _currentPlayer?.IsPlaying == true;

        public List<PlaylistItem> Playlist { get; } = new List<PlaylistItem>();

        public PlaylistItem UpcomingItem => Playlist?.FirstOrDefault(p => p.State == PlaylistState.Queued);

        public event EventHandler<PlayerPositionEventArgs> PlayerPositionChanged;
        public event EventHandler<PlayStateChangedEventArgs> PlaystateChanged;

        public PlaybackService(IConnectionManager connectionManager, IServerInfoService serverInfo)
        {
            _connectionManager = connectionManager;
            _serverInfo = serverInfo;
        }


        public void AddToPlaylist(IList<BaseItemDto> items)
        {
            Playlist.AddRange(items.Select(x => new PlaylistItem(x)));
        }

        public void AddToPlaylist(BaseItemDto item)
        {
            Playlist.Add(new PlaylistItem(item));
        }

        public void DecreaseVolume()
        {
            _currentPlayer?.DecreaseVolume();
        }

        public void IncreaseVolume()
        {
            _currentPlayer?.IncreaseVolume();
        }

        public void Pause()
        {
            _currentPlayer?.Stop();
        }

        public Task<bool> PlayItem(BaseItemDto item, long position = 0)
        {
            var playlistItem = new PlaylistItem(item);
            Playlist.Clear();      
            Playlist.Add(playlistItem);

            return PlayItem(playlistItem, position);
        }

        public Task<bool> PlayItems(IList<BaseItemDto> items)
        {
            Playlist.Clear();
            Playlist.AddRange(items.Select(x => new PlaylistItem(x)));
            return PlayItem(Playlist.FirstOrDefault(), 0);
        }

        public async Task<bool> PlayItems(string[] itemIds, long? position)
        {
            foreach (string item in itemIds)
            {
                var dto = await _apiClient?.GetItemAsync(item, _serverInfo?.ServerInfo?.Id);
                if (dto != null)
                    Playlist.Add(new PlaylistItem(dto));
            }
            return await PlayItem(GetNextItemFromPlaylist(), position ?? 0);
        }

        public bool RegisterPlayer(IMediaPlayer player)
        {
            bool registrationSucceded = false;
            if (!AvailablePlayers.Any(p => p.Id == player.Id))
            {
                AvailablePlayers.Add(player);
                registrationSucceded = true;
            }
            return registrationSucceded;
        }

        public void RemoveFromPlaylist(string itemId)
        {
            Playlist.RemoveAll(p => p.Item.Id == itemId);
        }

        public void RemoveFromPlaylist(PlaylistItem item)
        {
            Playlist.Remove(item);
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
            _currentPlayer.Pause();
        }

        public Task<bool> Seek(long newPosition)
        {
            return _currentPlayer?.Seek(newPosition);
        }

        public void SetAudioStreamIndex(int? index)
        {
            _currentPlayer?.SetAudioStreamIndex(index ?? 0);
        }

        public Task<bool> SetNext()
        {
            return PlayItem(GetNextItemFromPlaylist(), 0);
        }

        public Task<bool> SetPrevious()
        {
            return PlayItem(GetPreviousItemFromPlaylist(), 0);
        }

        public void SetSubtitleIndex(int? index)
        {
            _currentPlayer?.SetSubtitleStreamIndex(index);
        }

        public void SetVolume(double volume)
        {
            _currentPlayer?.SetVolume(volume);
        }

        public Task<bool> SkipToItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            _currentPlayer?.Stop();
        }

        private async Task<bool> PlayItem(PlaylistItem item, long position)
        {   
            var player = GetPlayerForItem(item.Item);
            if(player != null)
            {
                _currentPlayer?.Stop();
                SetItemAsCurrentItemPlaying(item);
                _currentPlayer = player;
                await player.Play(item.Item, position);
                return true;
            }
            return false;
        }

        private PlaylistItem GetNextItemFromPlaylist()
        {
            return Playlist.FirstOrDefault(p => p.State == PlaylistState.Queued);
        }

        private PlaylistItem GetPreviousItemFromPlaylist()
        {
            return Playlist.LastOrDefault(p => p.State == PlaylistState.Played);
        }

        private void SetItemAsCurrentItemPlaying(PlaylistItem nextTrack)
        {
            var item = Playlist.FirstOrDefault(p => p.State == PlaylistState.Playing);
            if (item != null)
            {
                item.State = PlaylistState.Played;
            }
            nextTrack.State = PlaylistState.Playing;
        }

        private IMediaPlayer GetPlayerForItem(BaseItemDto item)
        {
            //Very basic solution, we will have to improve on this to get different players depending on codecs and containers etc
            if (item.IsAudio)
            {
                return AvailablePlayers.FirstOrDefault(p => p.PlayerType == PlayerType.Audio || p.PlayerType == PlayerType.AudioAndVideo);
            }
            else
            {
                return AvailablePlayers.FirstOrDefault(p => p.PlayerType == PlayerType.Video || p.PlayerType == PlayerType.AudioAndVideo);
            }
        }

    }
}
