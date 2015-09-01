using Emby.Mobile.Universal.Services;
using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupView
    {
        public StartupView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await HideSystemTrayAsync();
            base.OnNavigatedTo(e);
        }

        private static async Task HideSystemTrayAsync()
        {
            if (AppServices.DeviceInfo.DeviceFamily == Mobile.Core.Helpers.DeviceFamily.Mobile)
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
