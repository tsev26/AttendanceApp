using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecord : DomainObject
    {
        public User User { get; set; }
        public DateTime Entry { get; set; }
    }
}
