using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Emby.Mobile.Universal.Interfaces;
using Emby.Mobile.ViewModels;

namespace Emby.Mobile.Universal.Views
{
    public sealed partial class MainView : ICanHasHeaderMenu
    {
        private readonly List<string> HeaderList = new List<string>
        {
            Mobile.Core.Strings.Resources.HeaderHome,
            Mobile.Core.Strings.Resources.HeaderWatchList,
            Mobile.Core.Strings.Resources.HeaderFavourites
        };

        public MainView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                Header.SelectedItem = Header.Items.FirstOrDefault();
            };
        }

        private MainViewModel Main => DataContext as MainViewModel;

        private void Header_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Content.SelectedIndex = Header.SelectedIndex;
        }
    }
}
