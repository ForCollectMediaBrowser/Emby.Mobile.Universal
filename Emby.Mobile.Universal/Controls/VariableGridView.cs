using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    public class EmbyGridView : GridView
    {
        private static readonly Size PrimaryItem = new Size(2, 2);
        private static readonly Size SecondarySmallItem = new Size(1, 1);
        private static readonly List<Size> SequenceDefault = new List<Size>
                                                                    {
                                                                        PrimaryItem,
                                                                        SecondarySmallItem,
                                                                        SecondarySmallItem,
                                                                        SecondarySmallItem,
                                                                        SecondarySmallItem,
                                                                        SecondarySmallItem,
                                                                        SecondarySmallItem
                                                                    };

        public static readonly DependencyProperty IsVariableGridProperty = DependencyProperty.Register(
            "IsVariableGrid", typeof(bool), typeof(EmbyGridView), new PropertyMetadata(default(bool)));

        public bool IsVariableGrid
        {
            get { return (bool)GetValue(IsVariableGridProperty); }
            set { SetValue(IsVariableGridProperty, value); }
        }

        public static readonly DependencyProperty RepeatItemsProperty = DependencyProperty.Register(
            "RepeatItems", typeof(bool), typeof(EmbyGridView), new PropertyMetadata(default(bool)));

        public bool RepeatItems
        {
            get { return (bool)GetValue(RepeatItemsProperty); }
            set { SetValue(RepeatItemsProperty, value); }
        }

        public static readonly DependencyProperty SequenceProperty = DependencyProperty.Register(
            "Sequence", typeof(List<Size>), typeof(EmbyGridView), new PropertyMetadata(SequenceDefault));

        public List<Size> Sequence
        {
            get { return (List<Size>)GetValue(SequenceProperty); }
            set { SetValue(SequenceProperty, value); }
        }

        private int _rowVal;
        private int _colVal;
        private int _count;

        public int Columns { get; set; }
        public int Rows { get; set; }

        public EmbyGridView()
        {
            Loaded += EmbyGridView_Loaded;
            Columns = 2;
            Rows = 2;
        }

        private void EmbyGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _count = 0;
            Loaded -= EmbyGridView_Loaded;
        }

        protected override void OnItemsChanged(object e)
        {
            _count = 0;
            base.OnItemsChanged(e);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (IsVariableGrid)
            {
                TreatElement(element);
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        private void TreatElement(DependencyObject element)
        {
            if (RepeatItems)
            {
                if (_count >= Sequence.Count)
                {
                    _count = 0;
                }

                _colVal = (int) Sequence[_count].Width;
                _rowVal = (int) Sequence[_count].Height;
                _count++;
            }
            else
            {
                _count++;
                if (_count < 2 && _count < Sequence.Count)
                {
                    _colVal = Columns;
                    _rowVal = Rows;
                }
                else
                {
                    _colVal = (int) SecondarySmallItem.Width;
                    _rowVal = (int) SecondarySmallItem.Height;
                }
            }

            VariableSizedWrapGrid.SetRowSpan(element as UIElement, _rowVal);
            VariableSizedWrapGrid.SetColumnSpan(element as UIElement, _colVal);
        }
    }
}