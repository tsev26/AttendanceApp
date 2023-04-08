using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Attendance.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static void AutoResizeGridViewColumns(GridView view)
        {
            if (view == null || view.Columns.Count < 1) return;
            // Simulates column auto sizing
            foreach (var column in view.Columns)
            {
                // Forcing change
                if (double.IsNaN(column.Width))
                    column.Width = 1;
                column.Width = double.NaN;
            }
        }
    }
}
