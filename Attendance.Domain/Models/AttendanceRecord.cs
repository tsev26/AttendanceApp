using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecord : DomainObject
    {
        public AttendanceRecord(User user, Activity activity, DateTime entry) : base()
        {
            User = user;
            Activity = activity;
            Entry = entry;
        }

        public User User { get; set; }
        public Activity Activity { get; set; }
        public DateTime Entry { get; set; }
    }
}
