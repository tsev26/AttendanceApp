using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Functions
{
    public static class TimeFunction
    {
        public static string ConvertSecondsToHHMMSS(long totalSeconds)
        {
            long hours = totalSeconds / 3600;
            long minutes = (totalSeconds % 3600) / 60;
            long seconds = totalSeconds % 60;

            string hoursStr = hours.ToString();
            string minutesStr = minutes.ToString().PadLeft(2, '0');
            string secondsStr = seconds.ToString().PadLeft(2, '0');

            return hoursStr + ":" + minutesStr + ":" + secondsStr;
        }
    }
}
