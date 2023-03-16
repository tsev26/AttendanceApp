using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Attendance.WPF.Views
{
    public partial class UserSelectActivityView : UserControl
    {
        public UserSelectActivityView()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, SelectActivityTextBox);
            Keyboard.Focus(SelectActivityTextBox);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SelectActivityTextBox);
            //SelectActivityTextBox.Focus();
        }

        private void ActivitiesListView_GotFocus(object sender, RoutedEventArgs e)
        {
            var selector = sender as Selector;
            if (selector != null)
            {
                var container = selector.ContainerFromElement((DependencyObject)e.OriginalSource);
                if (container != null)
                {
                    selector.SelectedItem = selector.ItemContainerGenerator.ItemFromContainer(container);
                }
            }
            SelectActivityTextBox.Focus();
        }
    }
}
