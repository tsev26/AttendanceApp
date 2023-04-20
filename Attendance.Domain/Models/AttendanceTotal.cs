using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceTotal : DomainObject
    {
        public AttendanceTotal(User user, DateOnly date, Activity activity, TimeSpan duration) : base()
        {
            User = user;
            UserId = user?.ID ?? 0;
            Date = date;
            Activity = activity;
            ActivityId = activity.ID;
            Duration = duration;
        }

        public AttendanceTotal() : base() { }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }

        [ForeignKey("ActivityId")]
        public Activity? Activity { get; set; }
        public int ActivityId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
