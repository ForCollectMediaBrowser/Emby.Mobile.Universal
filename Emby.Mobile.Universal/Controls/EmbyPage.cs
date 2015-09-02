using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        public EmbyPage()
        {
            Analytics = SimpleIoc.Default.GetInstance<IAnalyticsService>();
            Loaded += (sender, args) =>
            {
                Header = ViewModelLocator.Get<HeaderMenuViewModel>();
            };
        }

        public override INavigationService NavigationService { get; } = AppServices.NavigationService;

        protected IAnalyticsService Analytics { get; }

        protected HeaderMenuViewModel Header { get; private set; } 

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Analytics.PageLoad(GetType().Name);
            base.OnNavigatedTo(e);
        }
    }
}
