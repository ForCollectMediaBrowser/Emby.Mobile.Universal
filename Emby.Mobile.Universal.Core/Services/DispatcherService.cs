using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Emby.Mobile.Core.Interfaces;

namespace Emby.Mobile.Universal.Core.Services
{
    public class DispatcherService : IDispatcherService
    {
        public Task RunAsync(Action actionToRun)
        {
            var window = Window.Current;
            if (window != null)
            {
                return window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => actionToRun()).AsTask();
            }

            return Task.FromResult(0);
        }
    }
}
