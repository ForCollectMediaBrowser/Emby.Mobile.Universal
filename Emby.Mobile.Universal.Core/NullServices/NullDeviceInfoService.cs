using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullDeviceInfoService : IDeviceInfoService
    {
        public DeviceFamily DeviceFamily { get; } = DeviceFamily.Unknown;
        public bool SupportsBackButton { get; } = false;
        public bool SupportsStatusBar { get; } = false;
        public bool SupportsVibrate { get; } = false;
    }
}
