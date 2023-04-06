using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Attendance.WPF.Converters
{
    public class DisplayEnumNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prop = value.GetType().GetMember(value.ToString())[0].GetCustomAttribute<DisplayAttribute>();
            var displayAttribute = prop?.Name;
            if (displayAttribute != null)
            {
                return displayAttribute;
            }
            else
            {
                return prop.Name;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
