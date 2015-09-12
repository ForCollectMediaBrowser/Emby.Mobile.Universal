using Emby.Mobile.Core.Helpers;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IDeviceInfoService
    {
        bool SupportsBackButton { get; }
        bool SupportsVibrate { get; }
        bool SupportsStatusBar { get; }
        DeviceFamily DeviceFamily { get; }
        IDevice Device { get; }
        void SetName(string name);
    }
}
