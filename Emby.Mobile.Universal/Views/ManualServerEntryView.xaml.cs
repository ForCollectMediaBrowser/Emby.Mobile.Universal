using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManualServerEntryView
    {
        public ManualServerEntryView()
        {
            this.InitializeComponent();
        }

        private ManualServerEntryViewModel ManualServerEntry => DataContext as ManualServerEntryViewModel;
    }
}
