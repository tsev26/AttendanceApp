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
    public class ConditionForDaysVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value1 = values[0];
            var value2 = values[1];
            var value3 = values[2];
            var value4 = values[3];
            var value5 = values[4];
            var value6 = values[5];
            var value7 = values[6];
            var value8 = values[7];
            var value9 = values[8];
            var value10 = values[9];
            var value11 = values[10];
            var value12 = values[11];
            var value13 = values[12];
            var value14 = values[13];


            if (value1 is bool boolValue1 && value2 is bool boolValue2 && 
                value3 is bool boolValue3 && value4 is bool boolValue4 && 
                value5 is bool boolValue5 && value6 is bool boolValue6 && 
                value7 is bool boolValue7 && value8 is bool boolValue8 &&
                value9 is bool boolValue9 && value10 is bool boolValue10 &&
                value11 is bool boolValue11 && value12 is bool boolValue12 &&
                value13 is bool boolValue13 && value14 is bool boolValue14)
            {
                if (boolValue1 != boolValue2 ||
                    boolValue3 != boolValue4 ||
                    boolValue5 != boolValue6 ||
                    boolValue7 != boolValue8 ||
                    boolValue9 != boolValue10 ||
                    boolValue11 != boolValue12 ||
                    boolValue13 != boolValue14)
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