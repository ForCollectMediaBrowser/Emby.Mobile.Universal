using System;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Interfaces
{
    public interface IDispatcherService
    {
        Task RunAsync(Action actionToRun);
    }
}
