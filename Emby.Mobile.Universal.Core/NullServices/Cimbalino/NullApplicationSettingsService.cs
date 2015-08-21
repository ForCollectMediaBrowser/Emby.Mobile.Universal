using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices.Cimbalino
{
    public class NullApplicationSettingsService : IApplicationSettingsService
    {
        public IApplicationSettingsServiceHandler Local { get; } = null;
        public IApplicationSettingsServiceHandler Roaming { get; } = null;
        public IApplicationSettingsServiceHandler Legacy { get; } = null;
    }
}