using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class ActivityProperty : DomainObject
    {
        public ActivityProperty(bool isPlan, 
                                bool count, 
                                bool isPause, 
                                bool hasPause, 
                                TimeSpan maxInDay,
                                string groupByName) : base()
        {
            IsPlan = isPlan;
            Count = count;
            IsPause = isPause;
            HasPause = hasPause;
            MaxInDay = maxInDay;
            GroupByName = groupByName;
        }

        public ActivityProperty(ActivityProperty activityProperty) : base()
        {
            Id = activityProperty.Id;
            IsPlan = activityProperty.IsPlan;
            Count = activityProperty.Count;
            IsPause = activityProperty.IsPause;
            HasPause = activityProperty.HasPause;
            MaxInDay = activityProperty.MaxInDay;
            GroupByName = activityProperty.GroupByName;
        }

        public bool IsPlan { get; set; }
        public bool Count { get; set; }
        public bool IsPause { get; set; }
        public bool HasPause { get; set; }
        public TimeSpan MaxInDay { get; set; }
        public string GroupByName { get; set; }
        public bool IsWork => !IsPause;
        public ActivityProperty Clone()
        {
            return new ActivityProperty(this)
            {
                Id = this.Id
            };
        }
    }
}
