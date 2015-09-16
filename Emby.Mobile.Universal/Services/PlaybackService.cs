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
using MediaBrowser.ApiInteraction.Playback;
using MediaBrowser.Model.Dlna;
using Emby.Mobile.Core.Extensions;

namespace Emby.Mobile.Universal.Services
{
    internal class PlaybackService : IPlaybackService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConnectionManager _connectionManager;
        private readonly IPlaybackManager _playbackManager;
        private readonly IServerInfoService _serverInfo;
        private DateTime _lastProgressReportTimeStamp = DateTime.MinValue;
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

        public PlaybackService(IConnectionManager connectionManager, IServerInfoService serverInfo, IAuthenticationService authenticationService, IPlaybackManager playbackManager)
        {
            _connectionManager = connectionManager;
            _playbackManager = playbackManager;
            _serverInfo = serverInfo;
            _authenticationService = authenticationService;
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
            var playlist = new List<PlaylistItem> { new PlaylistItem(item) };
            return PlayItems(playlist, position);
        }

        public Task<bool> PlayItems(IList<BaseItemDto> items, int? startingItem = null)
        {
            var playlist = new List<PlaylistItem>();
            playlist.AddRange(items.Select(x => new PlaylistItem(x)));
            return PlayItems(playlist, 0, startingItem);
        }

        public async Task<bool> PlayItems(string[] itemIds, long? position)
        {
            var playlist = new List<PlaylistItem>();
            var tasks = itemIds.Select(item => GetAndAddItem(item, playlist)).ToList();

            await Task.WhenAll(tasks);

            var resultList = playlist.ToLookup(x => x.Id.ToString());
            var sortedPlaylist = itemIds.SelectMany(id => resultList[id]).ToList();

            return await PlayItems(sortedPlaylist, position ?? 0);
        }

        private async Task GetAndAddItem(string item, IList<PlaylistItem> playlist)
        {
            var dto = await _apiClient.GetItemAsync(item, _serverInfo?.ServerInfo?.Id);
            if (dto != null)
            {
                playlist.Add(new PlaylistItem(dto));
            }
        }

        public bool RegisterPlayer(IMediaPlayer player)
        {
            bool registrationSucceded = false;
            if (AvailablePlayers.All(p => p.PlayerType != player.PlayerType))
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

        public async void ReportPlaybackProgress(PlaybackProgressInfo info, StreamInfo streamInfo)
        {
            if (DateTime.UtcNow > _lastProgressReportTimeStamp.AddSeconds(5))
            {
                _lastProgressReportTimeStamp = DateTime.UtcNow;
                await _playbackManager.ReportPlaybackProgress(info, streamInfo, _serverInfo.IsOffline, _apiClient);
            }
        }

        public async void ReportPlaybackStarted(PlaybackStartInfo info)
        {
            await _playbackManager.ReportPlaybackStart(info, _serverInfo.IsOffline, _apiClient);
        }

        public async void ReportPlaybackStopped(PlaybackStopInfo info, StreamInfo streamInfo)
        {
            await _playbackManager.ReportPlaybackStopped(info,
                                                         streamInfo,
                                                         _serverInfo.ServerInfo.Id,
                                                         _authenticationService.SignedInUserId,
                                                         _serverInfo.IsOffline,
                                                         _apiClient);
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
            return _currentPlayer?.SetNext();
        }

        public Task<bool> SetPrevious()
        {
            return _currentPlayer?.SetPrevious();
        }

        public void SetSubtitleIndex(int? index)
        {
            _currentPlayer?.SetSubtitleStreamIndex(index);
        }

        public void SetVolume(double volume)
        {
            _currentPlayer?.SetVolume(volume);
        }

        public void SkipToItem(string itemId)
        {
            var item = Playlist?.FirstOrDefault(i => i.Item.Id == itemId);
            if (item != null)
            {
                _currentPlayer?.SkipToItem(item);
            }
        }

        public void Stop()
        {
            _currentPlayer?.Stop();
        }

        private async Task<bool> PlayItems(List<PlaylistItem> items, long position, int? startingItem = null)
        {
            if (!items.IsNullOrEmpty())
            {
                var player = GetPlayerForItem(items.First().Item);
                if (player != null)
                {
                    _currentPlayer?.Stop();
                    _currentPlayer = player;
                    await player.Play(items, position, startingItem);
                    return true;
                }
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

        private IMediaPlayer GetPlayerForItem(BaseItemDto item)
        {
            IMediaPlayer player = null;
            //Very basic solution, we will have to improve on this to get different players depending on codecs and containers etc
            if (item.IsAudio)
            {
                player = AvailablePlayers.FirstOrDefault(p => p.PlayerType == PlayerType.Audio || p.PlayerType == PlayerType.AudioAndVideo);
            }
            
            if(item.IsVideo)
            {
                player = AvailablePlayers.FirstOrDefault(p => p.PlayerType == PlayerType.Video || p.PlayerType == PlayerType.AudioAndVideo);
            }

            if (item.Type == "Photo")
            {
                player = AvailablePlayers.FirstOrDefault(p => p.PlayerType == PlayerType.Image);
            }

            return player;
        }

        public void ReportPlaylistStatus(IList<PlaylistItem> playlist)
        {
            Playlist.Clear();
            Playlist.AddRange(playlist);
        }

        public void ReportPlayerState(PlayerState state)
        {
            PlaystateChanged?.Invoke(this, new PlayStateChangedEventArgs(state));
        }
    }
}
