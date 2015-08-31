namespace Emby.Mobile.Core.Interfaces
{
    public interface IStatusBarService
    {
        void DisplayIndeterminateStatus(string message);
        void DisplayMessage(string message);
        void DisplayError(string message);
        void DisplayWarning(string message);
        void DisplaySuccess(string message);
    }
}
