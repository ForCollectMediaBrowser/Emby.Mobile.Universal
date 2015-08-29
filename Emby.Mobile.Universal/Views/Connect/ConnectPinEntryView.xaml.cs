using Windows.UI.Xaml.Navigation;
using Emby.Mobile.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace Emby.Mobile.Universal.Views.Connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectPinEntryView
    {
        public ConnectPinEntryView()
        {
            InitializeComponent();
        }
        
        private ConnectPinEntryViewModel PinEntry => DataContext as ConnectPinEntryViewModel;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            PinEntry.Cancel();
            base.OnNavigatedFrom(e);
        }
    }
}
