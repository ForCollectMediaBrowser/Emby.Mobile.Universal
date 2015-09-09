using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Universal.Services;
using Emby.Mobile.Universal.ViewModel;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenericItemView
    {
        private GenericItemViewModel Generic => DataContext as GenericItemViewModel;

        public GenericItemView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                var item = e.Parameter as BaseItemDto;
                if (item != null)
                {
                    var vm = ViewModelLocator.Get<GenericItemViewModel>(item.Id);
                    vm.SetItem(item);

                    DataContext = vm;
                }
                else
                {
                    AppServices.NavigationService.GoBack();
                }
            }

            base.OnNavigatedTo(e);
        }
    }
}
