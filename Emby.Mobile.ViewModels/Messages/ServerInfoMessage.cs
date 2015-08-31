using Emby.Mobile.ViewModels.Entities;
using GalaSoft.MvvmLight.Messaging;

namespace Emby.Mobile.Messages
{
    public class ServerInfoMessage : MessageBase
    {
        public const string DeleteServerMsg = "DeleteServerMsg";

        public ServerInfoViewModel Server { get; private set; }
        public string Notification { get; private set; }

        public ServerInfoMessage(ServerInfoViewModel server, string notification)
        {
            Server = server;
            Notification = notification;
        }
    }
}
