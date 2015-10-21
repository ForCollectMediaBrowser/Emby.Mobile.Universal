using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    [TemplatePart(Name = MainContent, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = SilhouetteContent, Type = typeof(ContentPresenter))]
    [TemplateVisualState(GroupName = ContentLoadingGroup, Name = ContentLoadedState)]
    [TemplateVisualState(GroupName = ContentLoadingGroup, Name = ContentLoadingState)]
    public sealed class SilhouetteControl : ContentControl
    {
        private const string MainContent = "MainContent";
        private const string SilhouetteContent = "SilhouetteContent";
        private const string ContentLoadingGroup = "ContentLoadingGroup";
        private const string ContentLoadedState = "ContentLoaded";
        private const string ContentLoadingState = "ContentLoading";

        public static readonly DependencyProperty SilhouetteProperty = DependencyProperty.Register(
            "Silhouette", typeof (object), typeof (SilhouetteControl), new PropertyMetadata(default(object)));

        public object Silhouette
        {
            get { return GetValue(SilhouetteProperty); }
            set { SetValue(SilhouetteProperty, value); }
        }

        public static readonly DependencyProperty SilhouetteTemplateProperty = DependencyProperty.Register(
            "SilhouetteTemplate", typeof (DataTemplate), typeof (SilhouetteControl), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate SilhouetteTemplate
        {
            get { return (DataTemplate) GetValue(SilhouetteTemplateProperty); }
            set { SetValue(SilhouetteTemplateProperty, value); }
        }

        public static readonly DependencyProperty DataLoadedProperty = DependencyProperty.Register(
            "DataLoaded", typeof (bool), typeof (SilhouetteControl), new PropertyMetadata(default(bool), OnDataLoadedChanged));

        public bool DataLoaded
        {
            get { return (bool) GetValue(DataLoadedProperty); }
            set { SetValue(DataLoadedProperty, value); }
        }

        public SilhouetteControl()
        {
            DefaultStyleKey = typeof(SilhouetteControl);
        }

        private static void OnDataLoadedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((SilhouetteControl)sender)?.UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            var state = DataLoaded ? ContentLoadedState : ContentLoadingState;

            VisualStateManager.GoToState(this, state, true);
        }
    }
}
