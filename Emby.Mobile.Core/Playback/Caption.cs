namespace Emby.Mobile.Core.Playback
{
    public class Caption
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Payload { get; set; }
        public string Url { get; set; }
        public bool HasPayload => !string.IsNullOrEmpty(Payload);
    }
}
