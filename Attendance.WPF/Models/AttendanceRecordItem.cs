using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Models
{
    public class AttendanceRecordItem
    {
        public AttendanceRecordItem(AttendanceRecord attendanceRecord, Activity activity, string dateTime)
        {
            AttendanceRecord = attendanceRecord;
            Activity = activity;
            DateTime = dateTime;
        }

        public AttendanceRecord AttendanceRecord { get; set; }
        public Activity Activity { get; set; }
        public string DateTime { get; set; }
    }
}
