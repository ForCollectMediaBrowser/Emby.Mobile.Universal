using Emby.Mobile.Core.Helpers;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Core.Implementations;
using MediaBrowser.Model.ApiClient;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullDeviceInfoService : IDeviceInfoService
    {
        public DeviceFamily DeviceFamily { get; } = DeviceFamily.Unknown;
        public IDevice Device { get; } = new Device();
        public void SetName(string name)
        {
        }

        public int? GetDeviceScaleImageValue(int? value)
        {
            return value;
        }

        public bool SupportsBackButton { get; } = false;
        public bool SupportsStatusBar { get; } = false;
        public bool SupportsVibrate { get; } = false;
    }
}
