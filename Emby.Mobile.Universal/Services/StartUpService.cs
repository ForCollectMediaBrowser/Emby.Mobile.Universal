using System.Threading.Tasks;
using Windows.UI.Xaml;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Services
{
    public class StartupService : IStartUpService
    {
        public Task Startup()
        {
            var header = ViewModelLocator.Get<HeaderMenuViewModel>();
            header.Start();

            App.Frame?.LoadLazyItems();

            return Task.FromResult(9);
        }
    }
}