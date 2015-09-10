using System;

namespace Emby.Mobile.Universal.BackgroundAudio
{
    public class TrackModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Uri MediaUri { get; set; }
        public Uri AlbumArtUri { get; set; }
    }
}
