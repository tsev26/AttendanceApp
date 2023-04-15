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
                          double minHoursWorked, 
                          bool hasRegularWorkingTime, 
                          TimeOnly latestArival, 
                          TimeOnly earliestDeparture, 
                          bool worksMonday, 
                          bool worksTuesday, 
                          bool worksWednesday, 
                          bool worksThursday, 
                          bool worksFriday, 
                          bool worksSaturday, 
                          bool worksSunday, 
                          List<Activity> availableActivities) : base()
        {
            MinHoursWorked = minHoursWorked;
            HasRegularWorkingTime = hasRegularWorkingTime;
            LatestArival = latestArival;
            EarliestDeparture = earliestDeparture;
            WorksMonday = worksMonday;
            WorksTuesday = worksTuesday;
            WorksWednesday = worksWednesday;
            WorksThursday = worksThursday;
            WorksFriday = worksFriday;
            WorksSaturday = worksSaturday;
            WorksSunday = worksSunday;
            AvailableActivities = new List<Activity>(availableActivities);
        }

        public Obligation(Obligation obligation)
        {
            MinHoursWorked = obligation.MinHoursWorked;
            HasRegularWorkingTime = obligation.HasRegularWorkingTime;
            LatestArival = obligation.LatestArival;
            EarliestDeparture = obligation.EarliestDeparture;
            WorksMonday = obligation.WorksMonday;
            WorksTuesday = obligation.WorksTuesday;
            WorksWednesday = obligation.WorksWednesday;
            WorksThursday = obligation.WorksThursday;
            WorksFriday = obligation.WorksFriday;
            WorksSaturday = obligation.WorksSaturday;
            WorksSunday = obligation.WorksSunday;
            AvailableActivities = new List<Activity>(obligation.AvailableActivities);
        }

        public Obligation() 
        {
            MinHoursWorked = 0;
            HasRegularWorkingTime = false;
            AvailableActivities = new List<Activity>();
        }

        public double MinHoursWorked { get; set; }
        public bool HasRegularWorkingTime { get; set; }
        public TimeOnly LatestArival { get; set; }
        public TimeOnly EarliestDeparture { get; set; }
        public bool WorksMonday { get; set; }
        public bool WorksTuesday { get; set; }
        public bool WorksWednesday { get; set; }
        public bool WorksThursday { get; set; }
        public bool WorksFriday { get; set; }
        public bool WorksSaturday { get; set; }
        public bool WorksSunday { get; set; }
        public virtual List<Activity> AvailableActivities { get; set; } 

        public Obligation Clone()
        {
            return new Obligation(this)
            {
                Id = this.Id
            };
        }
    }
}
