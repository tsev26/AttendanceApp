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


        public AttendanceRecord(AttendanceRecord attendanceRecord) : base()
        {
            User = attendanceRecord.User;
            Activity = attendanceRecord.Activity;
            Entry = attendanceRecord.Entry;
            AttendanceRecordDetail = attendanceRecord.AttendanceRecordDetail;
        }


        public User User { get; set; }
        public Activity Activity { get; set; }
        public DateTime Entry { get; set; }

        public AttendanceRecordDetail? AttendanceRecordDetail { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AttendanceRecord record &&
                   Id == record.Id;
        }

        public static bool operator ==(AttendanceRecord a, AttendanceRecord b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(AttendanceRecord a, AttendanceRecord b)
        {
            return !(a == b);
        }

        public AttendanceRecord Clone()
        {
            return new AttendanceRecord(this)
            {
                Id = this.Id,
            };
        }

    }
}
