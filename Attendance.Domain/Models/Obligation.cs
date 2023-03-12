using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Obligation : DomainObject
    {
        public TimeSpan MinTimeWorked   { get; set; }
        public bool HasRregularWorkingTime { get; set; }
        public TimeOnly LatestArival { get; set; }
        public TimeOnly EarliestDeparture { get; set; }
        public bool WorksRegularly { get; set; }
        public bool WorksMonday { get; set; }
        public bool WorksTuesday { get; set; }
        public bool WorksWednesday { get; set; }
        public bool WorksThursday { get; set; }
        public bool WorksFriday { get; set; }
        public bool WorksSaturday { get; set; }
        public bool WorksSunday { get; set; }
        public List<Activity> AvailableActivities { get; set; }
    }
}
