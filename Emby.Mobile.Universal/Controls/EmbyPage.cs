using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Services;
using GalaSoft.MvvmLight.Ioc;
using INavigationService = Cimbalino.Toolkit.Services.INavigationService;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        public EmbyPage()
        {
            Analytics = SimpleIoc.Default.GetInstance<IAnalyticsService>();
        }

        public override INavigationService NavigationService { get; } = AppServices.NavigationService;

        protected IAnalyticsService Analytics { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Analytics.PageLoad(GetType().Name);
            base.OnNavigatedTo(e);
        }
    }
}
