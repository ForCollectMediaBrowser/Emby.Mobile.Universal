using System;
using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.NullServices
{
    public class NullStartUpService : IStartUpService
    {
        public Task Startup()
        {
            throw new NotImplementedException();
        }
    }
}