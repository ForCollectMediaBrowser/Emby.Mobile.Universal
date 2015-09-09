//*********************************************************
using System;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public class AppResumedMessage
    {
        public AppResumedMessage()
        {
            Timestamp = DateTime.Now;
        }

        public AppResumedMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp;
    }
}
