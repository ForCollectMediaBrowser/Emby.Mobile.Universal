using Emby.Mobile.Core.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace Emby.Mobile.Universal.Services
{
    public class MessengerService : IMessengerService
    {
        public void SendAppResetNotification()
        {
            Messenger.Default.Send(new NotificationMessage("ResetAppMsg"));
        }

        public void SendNotification(string notification)
        {
            Messenger.Default.Send(new NotificationMessage(notification));
        }
        public void SendNotification(string notification, object sender)
        {
            Messenger.Default.Send(new NotificationMessage(sender, notification));
        }
    }
}
