using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecord : DomainObject
    {
        public AttendanceRecord(User user, Activity activity, DateTime entry, AttendanceRecordDetail? attendanceRecordDetail = null) : base()
        {
            User = user;
            Activity = activity;
            Entry = entry;
            AttendanceRecordDetail = attendanceRecordDetail;
        }

        public User User { get; set; }
        public Activity Activity { get; set; }
        public DateTime Entry { get; set; }

        public AttendanceRecordDetail? AttendanceRecordDetail { get; set; }
    }
}
