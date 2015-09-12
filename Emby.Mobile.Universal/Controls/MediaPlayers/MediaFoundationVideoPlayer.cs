using Emby.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dto;
using Windows.UI.Xaml;
using Emby.Mobile.Universal.Services;
using MediaBrowser.Model.Session;
using MediaBrowser.Model.Dlna;
using Emby.Mobile.Universal.Extensions;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.ApiInteraction.Playback;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.ApiInteraction.Data;
using System.IO;
using MediaBrowser.Model.Net;
using Emby.Mobile.Universal.Core.Helpers;
using Windows.Media.Core;
using MediaBrowser.Model.Entities;
using Windows.Media.Playback;
using Windows.UI.Xaml.Media;
using System.Linq;

namespace Emby.Mobile.Universal.Controls.MediaPlayers
{
    [TemplatePart(Name = "Player", Type = typeof(MediaElement))]
    public class MediaFoundationVideoPlayer : Control, IMediaPlayer
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IPlaybackManager _playbackManager;
        private readonly DispatcherTimer _postionChangedTimer;
        private readonly List<PlaylistItem> _playlist;
        private StreamInfo _streamInfo;
        private BaseItemDto _item;
        private MediaElement _player;

        public bool CanPause => _player?.CanPause == true;

        public bool CanSeek => _player?.CanSeek == true;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsPlaying => _player?.PlaybackRate > 0;

        public PlayerType PlayerType => PlayerType.Video;

        public PlayerState PlayerState => _player?.CurrentState.ToPlayerState() ?? PlayerState.Unknown;

        public MediaFoundationVideoPlayer()
        {
            _postionChangedTimer = new DispatcherTimer();
            _postionChangedTimer.Interval = TimeSpan.FromSeconds(1);
            _postionChangedTimer.Tick += PostionChangedTimer_Tick;
            _connectionManager = SimpleIoc.Default.GetInstance<IConnectionManager>();
            _playbackManager = SimpleIoc.Default.GetInstance<IPlaybackManager>();
            _playlist = new List<PlaylistItem>();
            DefaultStyleKey = typeof(MediaFoundationVideoPlayer);
        }

        private void PostionChangedTimer_Tick(object sender, object e)
        {
            AppServices.PlaybackService.ReportPlaybackProgress(new PlaybackProgressInfo
            {
                ItemId = _item.Id,
                CanSeek = CanSeek,
                AudioStreamIndex = null,
                IsPaused = _player.PlaybackRate == 0,
                IsMuted = _player.IsMuted,
                VolumeLevel = Convert.ToInt32(_player.Volume),
                PositionTicks = _player.Position.Ticks,
                PlayMethod = _streamInfo.PlayMethod
            }, _streamInfo);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _player = GetTemplateChild("Player") as MediaElement;
            _player.CurrentStateChanged += Player_CurrentStateChanged;
            _player.MediaOpened += Player_MediaOpened;
            _player.MediaEnded += Player_MediaEnded;
            _player.MediaFailed += Player_MediaFailed;

            AppServices.PlaybackService.RegisterPlayer(this);
        }

        private void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            _postionChangedTimer.Stop();
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            _postionChangedTimer.Stop();
            AppServices.DispatcherService.RunAsync(() =>
            {
                AppServices.PlaybackService.ReportPlaybackStopped(new PlaybackStopInfo
                {
                    ItemId = _item.Id,
                    PositionTicks = _player.Position.Ticks
                }, _streamInfo);
            });
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            AppServices.DispatcherService.RunAsync(() =>
            {
                AppServices.PlaybackService.ReportPlaybackStarted(new PlaybackStartInfo
                {
                    ItemId = _item.Id,
                    CanSeek = CanSeek,
                    AudioStreamIndex = null,
                    IsPaused = _player.PlaybackRate == 0,
                    IsMuted = _player.IsMuted,
                    VolumeLevel = Convert.ToInt32(_player.Volume),
                    PositionTicks = _player.Position.Ticks
                });
            });
            _postionChangedTimer.Start();
        }

        private void Player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            AppServices.DispatcherService.RunAsync(() =>
            {
                AppServices.PlaybackService.ReportPlayerState(MediaElementState.Playing.ToPlayerState());
            });
        }

        public void DecreaseVolume()
        {
            if (_player != null)
            {
                _player.Volume--;
            }
        }

        public void IncreaseVolume()
        {
            if (_player != null)
            {
                _player.Volume++;
            }
        }

        public void Pause()
        {
            if (CanPause)
            {
                _player.Pause();
            }
        }

        public Task Play(PlaylistItem item, double position = 0)
        {
            _playlist.Clear();
            _playlist.Add(item);
            return PlayItem(GetNextItem(), (int)position);
        }

        public Task Play(List<PlaylistItem> items, double position = 0)
        {
            _playlist.Clear();
            _playlist.AddRange(items);
            return PlayItem(GetNextItem(), (int)position);
        }

        public Task Add(List<PlaylistItem> items)
        {
            _playlist.AddRange(items);
            AppServices.PlaybackService.ReportPlaylistStatus(_playlist);
            return Task.FromResult(0);
        }

        public Task Remove(PlaylistItem item)
        {
            _playlist.RemoveAll(x => x.Id == item.Id);
            AppServices.PlaybackService.ReportPlaylistStatus(_playlist);
            return Task.FromResult(0);
        }

        public Task SkipToItem(PlaylistItem item)
        {
            var current = _playlist.FirstOrDefault(x => x.State == PlaylistState.Playing);
            if (current != null)
            {
                current.State = PlaylistState.Played;
            }

            item.State = PlaylistState.Playing;
            return PlayItem(item, 0);
        }

        public Task<bool> Seek(long positionTicks)
        {
            if (CanSeek)
            {
                _player.Position = TimeSpan.FromTicks(positionTicks);
            }
            return Task.FromResult(true);
        }

        public Task<bool> SetNext()
        {
            return PlayItem(GetNextItem(), 0);
        }

        public Task<bool> SetPrevious()
        {
            return PlayItem(GetPreviousItem(), 0);
        }

        public void SetVolume(double value)
        {
            if (_player != null)
            {
                _player.Volume = value;
            }
        }

        public void Stop()
        {
            _player?.Stop();
        }

        public void UnPause()
        {
            if (CanPause && _player.PlaybackRate == 0)
            {
                _player.Pause();
            }
        }

        public void NextAudioStream()
        {
            if (_player != null)
            {
                _player.AudioStreamIndex++;
            }
        }

        public void NextSubtitleStream()
        {
            //TODO Is this possible?
        }

        public void SetAudioStreamIndex(int audioStreamIndex)
        {
            if (_player != null)
            {
                _player.AudioStreamIndex = audioStreamIndex;
            }
        }

        public void SetSubtitleStreamIndex(int? subtitleStreamIndex)
        {
            if (_player != null)
            {
                //TODO Is this possible?
            }
        }

        private async Task<bool> PlayItem(PlaylistItem item, int positionTicks)
        {
            if (_player == null)
            {
                throw new NullReferenceException("Player is not initialized");
            }

            AppServices.PlaybackService.ReportPlaylistStatus(_playlist);

            if (item == null)
            {
                //Nothing to play;
                return false;
            }

            Width = double.NaN;
            Height = double.NaN;
            Opacity = 1;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            _player.AreTransportControlsEnabled = true;

            var client = SimpleIoc.Default.GetInstance<IConnectionManager>().GetApiClient(item.Item);
            var profile = await ConnectionManagerFactory.GetProfileAsync();
            _item = item.Item;
            _streamInfo = await item.GetStreamInfoAsync(positionTicks,
                                                   SimpleIoc.Default.GetInstance<IServerInfoService>().IsOffline,
                                                   _playbackManager,
                                                   client,
                                                   profile);

            var audio = _playbackManager.GetInPlaybackSelectableAudioStreams(_streamInfo);
            var captions = _playbackManager.GetInPlaybackSelectableSubtitleStreams(_streamInfo);

            IMediaPlaybackSource source;
            if (_streamInfo.PlayMethod == PlayMethod.DirectPlay && _streamInfo.MediaSource?.Protocol == MediaProtocol.File)
            {
                source = await GetLocalFileSourceAsync(captions);
            }
            else
            {
                source = GetRemoteSource(client, captions);
            }

            if (source != null)
            {
                _player.SetPlaybackSource(source);
                return true;
            }
            return false;
        }

        private async Task<IMediaPlaybackSource> GetLocalFileSourceAsync(IEnumerable<MediaStream> captions)
        {
            var assetManager = SimpleIoc.Default.GetInstance<ILocalAssetManager>();

            var stream = await assetManager.GetFileStream(_streamInfo);
            var mediaSource = MediaSource.CreateFromStream(stream.AsRandomAccessStream(), MimeTypes.GetMimeType("." + _streamInfo.Container));

            foreach (var caption in captions)
            {
                try
                {
                    var captionstream = await assetManager.GetFileStream(caption.Path);
                    mediaSource.ExternalTimedTextSources.Add(TimedTextSource.CreateFromStream(captionstream.AsRandomAccessStream(), caption.Language));
                }
                catch
                { }
            }

            return mediaSource;
        }

        private IMediaPlaybackSource GetRemoteSource(IApiClient client, IEnumerable<MediaStream> captions)
        {
            var url = _streamInfo.ToUrl(client.GetApiUrl("/"), client.AccessToken);
            var source = new Uri(url);
            var mediaSource = MediaSource.CreateFromUri(source);
            foreach (var caption in captions)
            {
                mediaSource.ExternalTimedTextSources.Add(TimedTextSource.CreateFromUri(GetSubtitleUri(_streamInfo, client, caption), caption.Language));
            }

            return mediaSource;
        }

        private Uri GetSubtitleUri(StreamInfo info, IApiClient client, MediaStream caption)
        {
            //TODO Add this functionality to the IApiClient instead
            return new Uri(string.Format("{0}/mediabrowser/Videos/{1}/{2}/Subtitles/{3}/Stream.vtt", client.ServerAddress, info.ItemId, info.MediaSourceId, caption.Index));
        }

        private PlaylistItem GetNextItem()
        {
            var currentItem = _playlist.FirstOrDefault(x => x.State == PlaylistState.Playing);
            if (currentItem != null)
            {
                currentItem.State = PlaylistState.Played;
            }

            var nextItem = _playlist.FirstOrDefault(x => x.State == PlaylistState.Queued);
            if (nextItem != null)
            {
                nextItem.State = PlaylistState.Playing;
            }
            return nextItem;
        }

        private PlaylistItem GetPreviousItem()
        {

            var currentItem = _playlist.FirstOrDefault(x => x.State == PlaylistState.Playing);
            if (currentItem != null)
            {
                currentItem.State = PlaylistState.Queued;
            }

            var previousItem = _playlist.FirstOrDefault(x => x.State == PlaylistState.Played);
            if (previousItem != null)
            {
                previousItem.State = PlaylistState.Playing;
            }
            return previousItem;
        }
    }
}
