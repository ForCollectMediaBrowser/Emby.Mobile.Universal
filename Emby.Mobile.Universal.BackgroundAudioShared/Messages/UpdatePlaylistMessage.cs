using System.Collections.Generic;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class UpdatePlaylistMessage
    {
        public List<TrackModel> Tracks { get; set; }
        public bool ClearCurrentList { get; set; }

        public UpdatePlaylistMessage(List<TrackModel> tracks, bool clearCurrentList)
        {
            Tracks = tracks ?? new List<TrackModel>();
            ClearCurrentList = clearCurrentList;
        }
    }
}
