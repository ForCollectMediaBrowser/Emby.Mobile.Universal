using System;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;

namespace Emby.Mobile.Universal.Core.NullServices.Cimbalino
{
    public class NullLauncherService : ILauncherService
    {
        public Task LaunchUriAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task LaunchUriAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task LaunchFileAsync(string file)
        {
            throw new NotImplementedException();
        }
    }
}
