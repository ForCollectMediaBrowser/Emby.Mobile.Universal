using System;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class AppSuspendedMessage
    {
        public DateTime Timestamp { get; }

        public AppSuspendedMessage()
        {
            Timestamp = DateTime.Now;
        }

        public AppSuspendedMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
