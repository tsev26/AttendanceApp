using System;
using System.Collections.Generic;
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
    }
}
