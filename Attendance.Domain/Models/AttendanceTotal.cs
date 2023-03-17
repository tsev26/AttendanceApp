using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceTotal : DomainObject
    {
        public AttendanceTotal(User user, DateOnly date, Activity activity, TimeSpan duration)
        {
            User = user;
            Date = date;
            Activity = activity;
            Duration = duration;
        }

        public AttendanceTotal()
        {

        }

        public User User { get; set; }
        public DateOnly Date { get; set; }
        public Activity Activity { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
