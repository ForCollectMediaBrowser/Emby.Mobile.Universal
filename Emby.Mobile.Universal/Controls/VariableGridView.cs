using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Emby.Mobile.Universal.Controls
{
    public class VariableGridView : GridView
    {
        private static readonly Size PrimaryItem = new Size(2, 2);
        private static readonly Size SecondarySmallItem = new Size(1, 1);

        private int _rowVal;
        private int _colVal;
        private readonly List<Size> _sequence;

        public bool Repeat { get; set; }
        public int Count;

        public int Columns { get; set; }
        public int Rows { get; set; }

        public VariableGridView()
        {
            Loaded += VariableGridView_Loaded;
            Columns = 2;
            Rows = 2;
            _sequence = new List<Size>
            {
                PrimaryItem,
                SecondarySmallItem,
                SecondarySmallItem,
                SecondarySmallItem,
                SecondarySmallItem,
                SecondarySmallItem,
                SecondarySmallItem
            };
        }

        private void VariableGridView_Loaded(object sender, RoutedEventArgs e)
        {
            Count = 0;
            Loaded -= VariableGridView_Loaded;
        }

        protected override void OnItemsChanged(object e)
        {
            Count = 0;
            base.OnItemsChanged(e);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (Repeat)
            {
                if (Count >= _sequence.Count)
                {
                    Count = 0;
                }

                _colVal = (int)_sequence[Count].Width;
                _rowVal = (int)_sequence[Count].Height;
                Count++;
            }
            else
            {
                Count++;
                if (Count < 2 && Count < _sequence.Count)
                {
                    _colVal = Columns;
                    _rowVal = Rows;
                }
                else
                {
                    _colVal = (int)SecondarySmallItem.Width;
                    _rowVal = (int)SecondarySmallItem.Height;
                }
            }

            VariableSizedWrapGrid.SetRowSpan(element as UIElement, _rowVal);
            VariableSizedWrapGrid.SetColumnSpan(element as UIElement, _colVal);

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}