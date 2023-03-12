using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceTotal : DomainObject
    {
        public User User { get; set; }
        public DateOnly Date { get; set; }
        public Activity Activity { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
