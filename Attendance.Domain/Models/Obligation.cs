using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
                          bool worksSunday) : base()
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
        }

        public Obligation() 
        {
            MinHoursWorked = 0;
            HasRegularWorkingTime = false;
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

        public virtual User ObligationMember { get; set;  }

        public virtual Group ObligationGroup { get; set; }

        public Obligation Clone()
        {
            return new Obligation(this)
            {
                ID = this.ID
            };
        }

        public bool Equals(Obligation other)
        {
            if (other == null)
                return false;
            bool result = MinHoursWorked == other.MinHoursWorked &&
            HasRegularWorkingTime == other.HasRegularWorkingTime &&
            LatestArival == other.LatestArival &&
            EarliestDeparture == other.EarliestDeparture &&
            WorksMonday == other.WorksMonday &&
            WorksTuesday == other.WorksTuesday &&
            WorksWednesday == other.WorksWednesday &&
            WorksThursday == other.WorksThursday &&
            WorksFriday == other.WorksFriday &&
            WorksSaturday == other.WorksSaturday &&
            WorksSunday == other.WorksSunday;
            return result;
        }

        public static bool operator ==(Obligation a, Obligation b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Obligation a, Obligation b)
        {
            return !(a == b);
        }
    }
}
