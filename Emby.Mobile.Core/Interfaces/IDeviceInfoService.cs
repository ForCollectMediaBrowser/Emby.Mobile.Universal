using Emby.Mobile.Core.Helpers;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IDeviceInfoService
    {
        bool SupportsBackButton { get; }
        bool SupportsVibrate { get; }
        bool SupportsStatusBar { get; }
        DeviceFamily DeviceFamily { get; }
    }
}
