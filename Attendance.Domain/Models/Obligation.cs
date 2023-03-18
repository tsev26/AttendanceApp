using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Obligation : DomainObject
    {
        public Obligation(
                          TimeSpan minTimeWorked, 
                          bool hasRregularWorkingTime, 
                          TimeOnly latestArival, 
                          TimeOnly earliestDeparture, 
                          bool worksRegularly, 
                          bool worksMonday, 
                          bool worksTuesday, 
                          bool worksWednesday, 
                          bool worksThursday, 
                          bool worksFriday, 
                          bool worksSaturday, 
                          bool worksSunday, 
                          List<Activity> availableActivities) : base()
        {
            MinTimeWorked = minTimeWorked;
            HasRregularWorkingTime = hasRregularWorkingTime;
            LatestArival = latestArival;
            EarliestDeparture = earliestDeparture;
            WorksRegularly = worksRegularly;
            WorksMonday = worksMonday;
            WorksTuesday = worksTuesday;
            WorksWednesday = worksWednesday;
            WorksThursday = worksThursday;
            WorksFriday = worksFriday;
            WorksSaturday = worksSaturday;
            WorksSunday = worksSunday;
            AvailableActivities = availableActivities;
        }

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
