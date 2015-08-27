using Emby.Mobile.Core.Helpers;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IDeviceInfoService
    {
        DeviceFamily DeviceFamily { get; }
    }
}
