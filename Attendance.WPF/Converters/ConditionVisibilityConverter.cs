using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Attendance.WPF.Converters
{
    public class ConditionVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value1 = values[0];
            var value2 = values[1];

            if (value1 is string strValue1 && value2 is string strValue2)
            {
                if (strValue1 != strValue2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (value1 is bool boolValue1 && value2 is bool boolValue2)
            {
                if (boolValue1 != boolValue2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (value1 is double doubleValue1 && value2 is double doubleValue2)
            {
                if (doubleValue1 != doubleValue2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (value1 is TimeOnly timeOnlyValue1 && value2 is TimeOnly timeOnlyValue2)
            {
                if (timeOnlyValue1 != timeOnlyValue2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
