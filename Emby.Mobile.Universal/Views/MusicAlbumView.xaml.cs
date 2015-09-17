using Windows.UI.Xaml.Navigation;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MusicAlbumView
    {
        private MusicAlbumViewModel Album => DataContext as MusicAlbumViewModel;

        public MusicAlbumView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            SetItem<MusicAlbumViewModel>(item);
            base.OnNavigatedTo(e);
        }
    }
}
