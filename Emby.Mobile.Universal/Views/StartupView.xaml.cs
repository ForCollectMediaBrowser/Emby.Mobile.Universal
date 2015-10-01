using Emby.Mobile.Universal.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private readonly SplashScreen _splashScreen;
        private StartupViewModel Startup => DataContext as StartupViewModel;

        public StartupView(SplashScreen splashScreen)
        {
            InitializeComponent();

            Loaded += StartupView_Loaded;

            Window.Current.SizeChanged += CurrentOnSizeChanged;

            _splashScreen = splashScreen;
            if (_splashScreen != null)
            {
                _splashScreen.Dismissed += SplashScreenOnDismissed;
                PositionImage();
            }
        }

        private void SplashScreenOnDismissed(SplashScreen sender, object args)
        {
            //PositionImage();
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            
        }

        private async void StartupView_Loaded(object sender, RoutedEventArgs e)
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

            Window.Current.SizeChanged -= CurrentOnSizeChanged;
            if (_splashScreen != null)
            {
                _splashScreen.Dismissed -= SplashScreenOnDismissed;
            }
        }
    }
}
