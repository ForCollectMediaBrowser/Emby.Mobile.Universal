using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Emby.Mobile.Universal.Services
{
    public class MessengerService : IMessengerService
    {
        public void SendAppResetNotification()
        {
            Messenger.Default.Send(new SignOutAppMessage());
        }

        public void SendNotification(string notification)
        {
            Messenger.Default.Send(new NotificationMessage(notification));
        }
    }
}
