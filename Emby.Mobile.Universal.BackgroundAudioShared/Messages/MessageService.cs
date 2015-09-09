using System.Diagnostics;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using Windows.Media.Playback;

namespace Emby.Mobile.Universal.BackgroundAudio.Messages
{
    public static class MessageService
    {
        private const string MessageType = "MessageType";
        private const string MessageBody = "MessageBody";

        public static void SendMessageToForeground<T>(T message)
        {
            var payload = new ValueSet();
            payload.Add(MessageType, typeof(T).FullName);
            payload.Add(MessageBody, JsonConvert.SerializeObject(message));
            BackgroundMediaPlayer.SendMessageToForeground(payload);
        }
    
        public static void SendMessageToBackground<T>(T message)
        {
            var payload = new ValueSet();
            payload.Add(MessageType, typeof(T).FullName);
            payload.Add(MessageBody, JsonConvert.SerializeObject(message));
            BackgroundMediaPlayer.SendMessageToBackground(payload);
        }

        public static bool TryParseMessage<T>(ValueSet valueSet, out T message)
        {
            object messageTypeValue;
            object messageBodyValue;

            message = default(T);
            if (valueSet.TryGetValue(MessageType, out messageTypeValue) && valueSet.TryGetValue(MessageBody, out messageBodyValue))
            {
                // Validate type
                if ((string)messageTypeValue != typeof(T).FullName)
                {
                    Debug.WriteLine("Message type was {0} but expected type was {1}", (string)messageTypeValue, typeof(T).FullName);
                    return false;
                }

                message = JsonConvert.DeserializeObject<T>(messageBodyValue.ToString());
                return true;
            }

            return false;
        }
    }
}
