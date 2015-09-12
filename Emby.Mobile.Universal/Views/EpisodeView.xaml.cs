using Windows.UI.Xaml.Navigation;
using Emby.Mobile.Core.Extensions;
using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.ViewModels;
using MediaBrowser.Model.Dto;

namespace Emby.Mobile.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EpisodeView : ICanHasHeaderMenu
    {
        private SeasonViewModel Season => DataContext as SeasonViewModel;

        public EpisodeView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter as BaseItemDto;
            SetItem<SeasonViewModel>(item, item?.ParentId);
            
            Season.SetSelectedEpisode(item).DontAwait("Just add this and let it do its thing (potentially getting episodes)");

            base.OnNavigatedTo(e);
        }
    }
}
