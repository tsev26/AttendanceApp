using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecordDetail : DomainObject
    {
        public AttendanceRecord AttendanceRecord { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public string Description { get; set; }

    }
}
