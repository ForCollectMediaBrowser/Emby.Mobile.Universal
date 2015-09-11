using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SeasonView : ICanHasHeaderMenu
    {
        private SeasonViewModel Season => DataContext as SeasonViewModel;
        private IItemSettable Item => DataContext as IItemSettable;

        public SeasonView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            SetItem<SeasonViewModel>(item);

            base.OnNavigatedTo(e);
        }
    }
}
