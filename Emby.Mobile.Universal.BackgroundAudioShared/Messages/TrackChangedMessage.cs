namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class TrackChangedMessage
    {
        public string Id { get; set; }

        public TrackChangedMessage()
        {
        }

        public TrackChangedMessage(string trackId)
        {
            Id = trackId;
        }
    }
}
