using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class ActivityProperty : DomainObject
    {
        public bool IsPlan { get; set; }
        public bool IsPause { get; set; }
        public bool HasPause { get; set; }
        public DateTime PauseEvery { get; set; }
        public DateTime PauseDuration { get; set; }
        public bool HasExpectedStart { get; set; }
        public bool HasExpectedReturn { get; set; }
        public bool IsFullDayActivity { get; set; }
        public bool IsHalfDayActivity { get; set; }
        public string GroupByName { get; set; }
    }
}
