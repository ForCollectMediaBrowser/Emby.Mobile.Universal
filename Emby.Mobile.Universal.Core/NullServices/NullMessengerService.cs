using System;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullMessengerService : IMessengerService
    {
        public void SendAppResetNotification()
        {
        }

        public void SendNotification(string notification)
        {
        }

        public void SendNotification(string notification, object sender)
        {
        }
    }
}
