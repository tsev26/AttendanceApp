using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class AttendanceRecordDetail : DomainObject
    {
        public AttendanceRecordDetail(DateTime expectedStart, DateTime expectedEnd, string description, bool isHalfDay = false) : base()
        {
            ExpectedStart = expectedStart;
            ExpectedEnd = expectedEnd;
            Description = description;
            IsHalfDay = isHalfDay;
        }

        public AttendanceRecordDetail() : base() { }

        public DateTime ExpectedStart { get; set; }
        public DateTime ExpectedEnd { get; set; }
        public bool IsHalfDay { get; set; } 
        public string Description { get; set; }

    }
}
