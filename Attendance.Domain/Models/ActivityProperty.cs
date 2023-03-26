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
                                TimeSpan pauseEvery, 
                                TimeSpan pauseDuration, 
                                bool hasExpectedStart, 
                                bool hasExpectedReturn, 
                                string groupByName) : base()
        {
            IsPlan = isPlan;
            Count = count;
            IsPause = isPause;
            HasPause = hasPause;
            PauseEvery = pauseEvery;
            PauseDuration = pauseDuration;
            HasExpectedStart = hasExpectedStart;
            HasExpectedReturn = hasExpectedReturn;
            GroupByName = groupByName;
        }

        public ActivityProperty(ActivityProperty activityProperty) : base()
        {
            Id = activityProperty.Id;
            IsPlan = activityProperty.IsPlan;
            Count = activityProperty.Count;
            IsPause = activityProperty.IsPause;
            HasPause = activityProperty.HasPause;
            PauseEvery = activityProperty.PauseEvery;
            PauseDuration = activityProperty.PauseDuration;
            HasExpectedStart = activityProperty.HasExpectedStart;
            HasExpectedReturn= activityProperty.HasExpectedReturn;
            GroupByName= activityProperty.GroupByName;
        }

        public bool IsPlan { get; set; }
        public bool Count { get; set; }
        public bool IsPause { get; set; }
        public bool HasPause { get; set; }
        public TimeSpan PauseEvery { get; set; }
        public TimeSpan PauseDuration { get; set; }
        public bool HasExpectedStart { get; set; }
        public bool HasExpectedReturn { get; set; }
        public string GroupByName { get; set; }

        public ActivityProperty Clone(ActivityProperty property)
        {
            return new ActivityProperty(property)
            {
                Id = property.Id
            };
        }
    }
}
