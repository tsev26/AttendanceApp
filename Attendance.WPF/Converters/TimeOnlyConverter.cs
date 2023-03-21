using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Attendance.WPF.Converters
{
    public class TimeOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert TimeOnly to String
            if (value is TimeOnly timeOnlyValue)
            {
                return timeOnlyValue.ToString();
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert String to TimeOnly
            if (value is string stringValue)
            {
                if (TimeOnly.TryParse(stringValue, out TimeOnly result))
                {
                    return result;
                }
            }

            return Binding.DoNothing;
        }
    }
}
