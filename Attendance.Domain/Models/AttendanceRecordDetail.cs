using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecordDetail : DomainObject
    {
        public AttendanceRecordDetail(DateTime expectedStart, DateTime expectedEnd, string description) : base()
        {
            ExpectedStart = expectedStart;
            ExpectedEnd = expectedEnd;
            Description = description;
        }

        public AttendanceRecordDetail() : base() { }

        public DateTime ExpectedStart { get; set; }
        public DateTime ExpectedEnd { get; set; }
        public string Description { get; set; }

    }
}
