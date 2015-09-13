using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Extensions;
using Emby.Mobile.ViewModels.UserViews;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views.UserViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TvUserView
    {
        private TvUserViewModel Tv => DataContext as TvUserViewModel;

        public TvUserView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            Tv?.OnNavigatedTo(e.NavigationMode.ExchangeMode(), item);

            base.OnNavigatedTo(e);
        }
    }
}
