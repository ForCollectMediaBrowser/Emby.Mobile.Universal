using Emby.Mobile.Universal.Services;
using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace Emby.Mobile.Universal.Views
{
    public sealed partial class StartupView
    {
        private StartupViewModel Startup => DataContext as StartupViewModel;

        public StartupView()
        {
            InitializeComponent();

            Loaded += StartupView_Loaded;
        }

        private async void StartupView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await AppServices.Create();

            HideSystemTrayAsync().DontAwait("Just get on with it, don't need to hold up the navigation");

            ViewModelLocator.RegisterEverything();

            SetServices(AppServices.Anayltics, AppServices.NavigationService);

            SimpleIoc.Default.GetInstance<IAuthenticationService>().Start();

            var vm = ViewModelLocator.Get<StartupViewModel>();
            DataContext = vm;

            Bindings.Update();

            vm.PageLoadedCommand.Execute(null);
        }

        private static async Task HideSystemTrayAsync()
        {
            if (AppServices.DeviceInfo.SupportsStatusBar)
            {
                var bar = StatusBar.GetForCurrentView();
                if (bar != null)
                {
                    bar.ForegroundColor = Windows.UI.Color.FromArgb(255, 82, 181, 75);
                    bar.BackgroundColor = Windows.UI.Colors.Transparent;
                    //By settings a progress value and displaying the Progressindicator, the clock will still be visible.
                    bar.ProgressIndicator.ProgressValue = 0;
                    bar.ProgressIndicator.Text = " ";
                    await bar.ProgressIndicator.ShowAsync();
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
        }
    }
}
