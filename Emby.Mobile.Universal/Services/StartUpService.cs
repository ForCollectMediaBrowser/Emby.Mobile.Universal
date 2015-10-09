using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Services
{
    public class StartupService : IStartUpService
    {
        private bool _frameStarted;

        public Task Startup()
        {
            var header = ViewModelLocator.Get<HeaderMenuViewModel>();
            header.Start();

            LoadFrame();

            return Task.FromResult(9);
        }

        public void LoadFrame()
        {
            if (!_frameStarted)
            {
                _frameStarted = App.Frame?.LoadLazyItems() ?? false;
            }
        }
    }
}