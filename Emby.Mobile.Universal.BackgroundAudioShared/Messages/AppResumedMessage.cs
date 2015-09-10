//*********************************************************
using System;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class AppResumedMessage
    {
        public DateTime Timestamp { get; }

        public AppResumedMessage()
        {
            Timestamp = DateTime.Now;
        }

        public AppResumedMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
