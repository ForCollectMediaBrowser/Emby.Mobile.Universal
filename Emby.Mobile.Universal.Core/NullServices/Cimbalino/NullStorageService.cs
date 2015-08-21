using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices.Cimbalino
{
    public class NullStorageService : IStorageService
    {
        public IStorageServiceHandler Local { get; } = null;
        public IStorageServiceHandler Roaming { get; } = null;
        public IStorageServiceHandler Temporary { get; } = null;
        public IStorageServiceHandler LocalCache { get; } = null;
        public IStorageServiceHandler Package { get; } = null;
    }
}