using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Attendance.Domain.Models
{
    public enum FixType
    {
        [Display(Name = "Úprava")]
        Update,

        [Display(Name = "Přidání")]
        Insert,

        [Display(Name = "Odebrání")]
        Delete
    }

    public enum ApproveType
    {
        [Display(Name = "Čeká")]
        Waiting,

        [Display(Name = "Schváleno")]
        Approved,

        [Display(Name = "Zamítnuto")]
        Rejected
    }

    public class AttendanceRecordFix : DomainObject
    {
        public AttendanceRecordFix() : base() { }
        public AttendanceRecordFix(AttendanceRecord attendanceRecord, User user, Activity activity, DateTime entry)
        {
            AttendanceRecord = attendanceRecord;

            User = user;
            Activity = activity;
            Entry = entry;

            FixType = FixType.Update;
            Approved = ApproveType.Waiting;
        }

        public AttendanceRecordFix(User user, Activity activity, DateTime entry)
        {
            User = user;
            Activity = activity;
            Entry = entry;

            FixType = FixType.Insert;
            Approved = ApproveType.Waiting;
        }

        public AttendanceRecordFix(AttendanceRecord attendanceRecord, User user)
        {
            AttendanceRecord = attendanceRecord;

            Activity = attendanceRecord.Activity;
            Entry = attendanceRecord.Entry;
            User = user;

            FixType = FixType.Delete;
            Approved = ApproveType.Waiting;
        }

        [ForeignKey("AttendanceRecordId")]
        public AttendanceRecord? AttendanceRecord { get; set; }
        public int AttendanceRecordId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
        public FixType FixType { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
        public DateTime Entry { get; set; }
        public ApproveType Approved { get; set; }
    }
}
