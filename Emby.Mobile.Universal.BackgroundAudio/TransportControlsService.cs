using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    internal class TransportControlsService
    {
        private SystemMediaTransportControls _transportControls;

        public event EventHandler<SystemMediaTransportControlsButtonPressedEventArgs> TransportControlsButtonPressed;

        public TransportControlsService()
        {
            _transportControls = BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            _transportControls.ButtonPressed += TransportControls_ButtonPressed;
            _transportControls.PropertyChanged += TransportControls_PropertyChanged;
            _transportControls.IsEnabled = true;
            _transportControls.IsPauseEnabled = true;
            _transportControls.IsPlayEnabled = true;
            _transportControls.IsNextEnabled = true;
            _transportControls.IsPreviousEnabled = true;
        }

        public void SetStatus(MediaPlaybackStatus status)
        {
            _transportControls.PlaybackStatus = status;
        }

        public void UpdateTransportControlsOnNewTrack(MediaPlaybackItem item)
        {
            if (item == null)
            {
                _transportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                _transportControls.DisplayUpdater.MusicProperties.Title = string.Empty;
                _transportControls.DisplayUpdater.Update();
                return;
            }

            _transportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
            _transportControls.DisplayUpdater.Type = MediaPlaybackType.Music;
            _transportControls.DisplayUpdater.MusicProperties.Title = item.Source.CustomProperties[BackgroundAudioConstants.Title] as string;

            var albumArtUri = item.Source.CustomProperties[BackgroundAudioConstants.AlbumArt] as Uri;
            if (albumArtUri != null)
                _transportControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(albumArtUri);
            else
                _transportControls.DisplayUpdater.Thumbnail = null;

            _transportControls.DisplayUpdater.Update();
        }

        public void Unregister()
        {
            _transportControls.ButtonPressed -= TransportControls_ButtonPressed;
            _transportControls.PropertyChanged -= TransportControls_PropertyChanged;
        }

        //Transport Controls
        private void TransportControls_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            if (args.Property == SystemMediaTransportControlsProperty.SoundLevel)
            {
                if (sender.SoundLevel == SoundLevel.Muted && BackgroundMediaPlayer.Current.PlaybackRate > 0)
                {
                    BackgroundMediaPlayer.Current.Pause();
                }
                else if (sender.SoundLevel != SoundLevel.Muted && BackgroundMediaPlayer.Current.PlaybackRate == 0)
                {
                    BackgroundMediaPlayer.Current.Play();
                }
            }

        }

        private void TransportControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            TransportControlsButtonPressed?.Invoke(sender, args);            
        }
    }
}
