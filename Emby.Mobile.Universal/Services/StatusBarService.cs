using Windows.UI.ViewManagement;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Controls;
using Emby.Mobile.Universal.Helpers;

namespace Emby.Mobile.Universal.Services
{
    public class StatusBarService : IStatusBarService
    {
        private readonly IDeviceInfoService _deviceInfo;
        private readonly Cimbalino.Toolkit.Services.IStatusBarService _statusBar = new Cimbalino.Toolkit.Services.StatusBarService();

        public StatusBarService(IDeviceInfoService deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }

        public void DisplayError(string message)
        {
            SetStatus(message, StatusType.Error);
        }

        public void DisplayMessage(string message)
        {
            SetStatus(message, StatusType.Message);
        }

        public void DisplayIndeterminateStatus(string message)
        {
            SetStatus(message, StatusType.Status);
        }

        public void DisplaySuccess(string message)
        {
            SetStatus(message, StatusType.Success);
        }

        public void DisplayWarning(string message)
        {
            SetStatus(message, StatusType.Warning);
        }

        private void SetStatus(string message, StatusType statusType)
        {
            if (_deviceInfo.SupportsStatusBar && statusType == StatusType.Status)
            {
                _statusBar.ShowAsync(message, !string.IsNullOrEmpty(message));
            }
            else
            {
                var statusBar = StatusBarControl.GetForCurrentView();
                statusBar?.Show(message, statusType);
            }
        }
    }
}
