using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullDispatcherService : IDispatcherService
    {
        public Task RunAsync(Action actionToRun)
        {
            throw new NotImplementedException();
        }
    }
}
