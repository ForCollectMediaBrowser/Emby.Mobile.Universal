using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Services;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using MediaBrowser.Model.Dto;
using ThemeManagerRt;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyPage : BasePage
    {
        private INavigationService _navigationService;
        public EmbyPage()
        {
            this.ThemeEnableThisElement();
        }

        public override INavigationService NavigationService => _navigationService;

        protected IAnalyticsService Analytics { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Analytics?.PageLoad(GetType().Name);

            var vm = DataContext as PageViewModelBase;
            vm?.OnNavigatedTo(e.NavigationMode.ExchangeMode(), e.Parameter as BaseItemDto);

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var vm = DataContext as PageViewModelBase;
            var canCancel = e.Cancel;
            vm?.OnNavigatingFrom(e.NavigationMode.ExchangeMode(), e.Parameter as BaseItemDto, ref canCancel);

            e.Cancel = canCancel;

            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var vm = DataContext as PageViewModelBase;
            vm?.OnNavigatedFrom(e.NavigationMode.ExchangeMode(), e.Parameter as BaseItemDto);

            base.OnNavigatedFrom(e);
        }

        protected virtual void SetItem<TViewModelType>(BaseItemDto item, string itemKey = null)
            where TViewModelType : IItemSettable
        {
            if (item != null)
            {
                var id = itemKey ?? item.Id;
                var vm = ViewModelLocator.Get<TViewModelType>(id);
                if (vm.Item == null)
                {
                    vm.SetItem(item);
                }

                DataContext = vm;
            }
            else
            {
                AppServices.NavigationService.GoBack();
            }
        }

        protected void SetServices(IAnalyticsService analytics, INavigationService navigationService)
        {
            _navigationService = navigationService;
            Analytics = analytics;
        }
    }
}
