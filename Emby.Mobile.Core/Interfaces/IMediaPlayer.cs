using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IMediaPlayer
    {
        Guid Id { get; }
        PlayerType PlayerType { get; }
        bool CanSeek { get; }
        bool CanPause { get; }
        bool IsPlaying { get; }

        Task Play(PlaylistItem item, double position = 0);
        Task Play(List<PlaylistItem> items, double position = 0);
        Task Add(List<PlaylistItem> items);
        Task Remove(PlaylistItem item);
        Task SkipToItem(PlaylistItem item);
        void Stop();
        void Pause();
        void UnPause();
        Task<bool> Seek(long positionTicks);
        void SetSubtitleStreamIndex(int? subtitleStreamIndex);
        void NextSubtitleStream();
        void SetAudioStreamIndex(int audioStreamIndex);
        void NextAudioStream();
        void IncreaseVolume();
        void DecreaseVolume();
        void SetVolume(double value);
    }
}
