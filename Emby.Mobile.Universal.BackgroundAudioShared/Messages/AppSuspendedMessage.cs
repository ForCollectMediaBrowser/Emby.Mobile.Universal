using System;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class AppSuspendedMessage
    {
        public AppSuspendedMessage()
        {
            Timestamp = DateTime.Now;
        }

        public AppSuspendedMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp;
    }
}
