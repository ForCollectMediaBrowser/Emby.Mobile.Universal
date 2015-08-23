namespace Emby.Mobile.Core.Interfaces
{
    public interface IMessengerService
    {
        void SendAppResetNotification();
        void SendNotification(string notification);
    }
}
