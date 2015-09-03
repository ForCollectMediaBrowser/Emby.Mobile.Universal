using System.Threading.Tasks;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Services
{
    public class StartUpService : IStartUpService
    {
        public Task Startup()
        {
            var header = ViewModelLocator.Get<HeaderMenuViewModel>();
            header.Start();

            return Task.FromResult(9);
        }
    }
}