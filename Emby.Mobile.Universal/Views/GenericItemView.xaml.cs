using Windows.UI.Xaml.Navigation;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class GenericItemView
    {
        private GenericItemViewModel Generic => DataContext as GenericItemViewModel;
        private IItemSettable Item => DataContext as IItemSettable;

        public GenericItemView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            SetItem<GenericItemViewModel>(item);

            base.OnNavigatedTo(e);
        }
    }
}
