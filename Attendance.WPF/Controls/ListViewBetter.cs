using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Attendance.WPF.Controls
{
    public class ListViewBetter : ListView
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ItemsSourceProperty)
            {
                GridView gridView = View as GridView;
                if (gridView != null)
                {
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        ResizeGridViewColumn(column);
                    }
                }
            }
        }

        
        private void ResizeGridViewColumn(GridViewColumn column)
        {
            if (double.IsNaN(column.Width))
            {
                column.Width = column.ActualWidth;
            }

            column.Width = double.NaN;
        }


        /*
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                SelectedIndex = -1;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += OnItemsCollectionChanged;
            }

            if (oldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnItemsCollectionChanged;
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                SelectedIndex = -1;
            }
        }
        */
    }
}
