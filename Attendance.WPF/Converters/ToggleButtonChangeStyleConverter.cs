using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Attendance.WPF.Converters
{
    public class ToggleButtonChangeStyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value1 = values[0];
            var value2 = values[1];

            ResourceDictionary resources = Application.Current.Resources;
            if (value1 is bool v1 && value2 is bool v2 && v1 != v2)
            {
                

                return resources["ToggleButtonChange"] as Style;
            }
            else
            {
                return resources[typeof(ToggleButton)] as Style;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
