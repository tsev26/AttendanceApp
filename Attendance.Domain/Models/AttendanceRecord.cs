using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecord : DomainObject
    {
        public AttendanceRecord() : base() { }
        public AttendanceRecord(User user, Activity activity, DateTime entry, AttendanceRecordDetail? attendanceRecordDetail = null) : base()
        {
            User = user;
            UserId = user.ID;
            Activity = activity;
            ActivityId = activity.ID;
            Entry = entry;
            AttendanceRecordDetail = attendanceRecordDetail;
        }


        public AttendanceRecord(AttendanceRecord attendanceRecord) : base()
        {
            User = attendanceRecord.User;
            UserId = attendanceRecord.User.ID;
            Activity = attendanceRecord.Activity;
            ActivityId = attendanceRecord.Activity.ID;
            Entry = attendanceRecord.Entry;
            AttendanceRecordDetail = attendanceRecord.AttendanceRecordDetail;
        }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity? Activity { get; set; }
        public int? ActivityId { get; set; }

        public DateTime Entry { get; set; }

        [ForeignKey("AttendanceRecordDetailInt")]
        public AttendanceRecordDetail? AttendanceRecordDetail { get; set; }
        public int? AttendanceRecordDetailInt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AttendanceRecord record &&
                   ID == record.ID;
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
                ID = this.ID,
            };
        }

    }
}
