using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Controls;
using Emby.Mobile.Universal.Helpers;

namespace Emby.Mobile.Universal.Services
{
    public class StatusBarService : IStatusBarService
    {
        public void DisplayError(string message)
        {
            StatusBarControl.GetForCurrentView()?.Show(message, StatusType.Error);
        }

        public void DisplayMessage(string message)
        {
            StatusBarControl.GetForCurrentView()?.Show(message, StatusType.Message);
        }

        public void DisplayIndeterminateStatus(string message)
        {
            StatusBarControl.GetForCurrentView()?.Show(message, StatusType.Status);
        }

        public void DisplaySuccess(string message)
        {
            StatusBarControl.GetForCurrentView()?.Show(message, StatusType.Success);
        }

        public void DisplayWarning(string message)
        {
            StatusBarControl.GetForCurrentView()?.Show(message, StatusType.Warning);
        }
    }
}
