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
    }
}
