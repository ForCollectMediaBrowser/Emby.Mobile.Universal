using Windows.System;
using Windows.UI.Xaml.Input;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views.Connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmbyConnectView 
    {
        public EmbyConnectView()
        {
            this.InitializeComponent();
        }

        private EmbyConnectViewModel EmbyConnect => DataContext as EmbyConnectViewModel;

        private void PasswordBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && EmbyConnect.CanSignIn)
            {
                EmbyConnect.SignInCommand.Execute(null);
            }
        }
    }
}
