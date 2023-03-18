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
                                bool isFullDayActivity, 
                                bool isHalfDayActivity, 
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
            IsFullDayActivity = isFullDayActivity;
            IsHalfDayActivity = isHalfDayActivity;
            GroupByName = groupByName;
        }

        public bool IsPlan { get; set; }
        public bool Count { get; set; }
        public bool IsPause { get; set; }
        public bool HasPause { get; set; }
        public TimeSpan PauseEvery { get; set; }
        public TimeSpan PauseDuration { get; set; }
        public bool HasExpectedStart { get; set; }
        public bool HasExpectedReturn { get; set; }
        public bool IsFullDayActivity { get; set; }
        public bool IsHalfDayActivity { get; set; }
        public string GroupByName { get; set; }
    }
}
