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
            var burger = ViewModelLocator.Get<BurgerMenuViewModel>();
            burger.Start();

            return Task.FromResult(9);
        }
    }
}