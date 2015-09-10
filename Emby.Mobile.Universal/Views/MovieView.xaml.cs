using Windows.UI.Xaml.Navigation;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieView
    {
        private MovieViewModel Movie => DataContext as MovieViewModel;
        private IItemSettable Item => DataContext as IItemSettable;

        public MovieView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            SetItem<MovieViewModel>(item);

            base.OnNavigatedTo(e);
        }
    }
}
