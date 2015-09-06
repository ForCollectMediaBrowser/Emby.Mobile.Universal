using Emby.Mobile.Core.Playback;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IMediaPlayer
    {
        PlayerType PlayerType { get; }
        bool CanSeek { get; }
        bool CanPause { get; }
        Task Play(BaseItemDto item, double position = 0);
        Task Play(string url, IList<Caption> captions, IList<MediaStream> selectableAudioStreams);
        Task Play(StreamInfo stream, MimeTypes mimeType, IList<Caption> captions, IList<MediaStream> selectableAudioStreams);
        void Pause();
        void UnPause();
        void Seek(long positionTicks);
        void SetSubtitleStreamIndex(int? subtitleStreamIndex);
        void NextSubtitleStream();
        void SetAudioStreamIndex(int audioStreamIndex);
        void NextAudioStream();
    }
}
