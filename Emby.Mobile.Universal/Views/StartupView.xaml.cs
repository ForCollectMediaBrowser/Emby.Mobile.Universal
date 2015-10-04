using Emby.Mobile.Universal.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Core.Interfaces;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.Universal.Helpers;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace Emby.Mobile.Universal.Views
{
    public sealed partial class StartupView
    {
        private SplashScreen _splashScreen;
        private StartupViewModel Startup => DataContext as StartupViewModel;

        public StartupView()
        {
            InitializeComponent();

            Loaded += StartupView_Loaded;
        }

        private async void StartupView_Loaded(object sender, RoutedEventArgs e)
        {
            await AppServices.Create();

            HideSystemTrayAsync().DontAwait("Just get on with it, don't need to hold up the navigation");

            TitleBarService.SetTitleBarColours();

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
                    bar.BackgroundColor = (Color?) Application.Current.Resources["EmbyGreenColor"];
                    bar.ForegroundColor = Colors.White;
                    bar.BackgroundOpacity = 1;
                    await bar.ShowAsync();//.DontAwait("Just show the status bar");
                    //By settings a progress value and displaying the Progressindicator, the clock will still be visible.
                    
                }
            }
        }

        private void PositionImage()
        {
            if (_splashScreen == null)
            {
                return;
            }

            var location = _splashScreen.ImageLocation;
            SplashScreenImage.SetValue(Canvas.LeftProperty, location.X);
            SplashScreenImage.SetValue(Canvas.TopProperty, location.Y);
            SplashScreenImage.Height = location.Height;
            SplashScreenImage.Width = location.Width;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var startupNavigationParemeters = e.Parameter as StartupNavigationParameter;
            if (startupNavigationParemeters != null)
            {
                _splashScreen = startupNavigationParemeters.SplashScreen;
                if (_splashScreen != null)
                {
                    PositionImage();
                }
            }
        }
    }
}
