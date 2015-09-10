namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class RemoveTrackMessage
    {
        public string Id { get; set; }

        public RemoveTrackMessage(string trackId)
        {
            Id = trackId;
        }
        
    }
}
