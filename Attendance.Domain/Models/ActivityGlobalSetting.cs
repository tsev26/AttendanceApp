using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class ActivityGlobalSetting : DomainObject
    {
        public ActivityGlobalSetting(ActivityGlobalSetting activityGlobalSetting) : base()
        {
            PauseEvery = activityGlobalSetting.PauseEvery;
            PauseDuration = activityGlobalSetting.PauseDuration;
            MainWorkActivity = activityGlobalSetting.MainWorkActivity;
            MainNonWorkActivity = activityGlobalSetting.MainNonWorkActivity;
            LenghtOfAllDayActivity = activityGlobalSetting.LenghtOfAllDayActivity;
            LenghtOfHalfDayActivity = activityGlobalSetting.LenghtOfHalfDayActivity;
        }

        public ActivityGlobalSetting(TimeSpan pauseEvery, 
                                     TimeSpan pauseDuration, 
                                     Activity mainWorkActivity, 
                                     Activity mainNonWorkActivity, 
                                     TimeSpan lenghtOfAllDayActivity, 
                                     TimeSpan lenghtOfHalfDayActivity) : base()
        {
            PauseEvery = pauseEvery;
            PauseDuration = pauseDuration;
            MainWorkActivity = mainWorkActivity;
            MainNonWorkActivity = mainNonWorkActivity;
            LenghtOfAllDayActivity = lenghtOfAllDayActivity;
            LenghtOfHalfDayActivity = lenghtOfHalfDayActivity;
        }

        public TimeSpan PauseEvery { get; set; }
        public TimeSpan PauseDuration { get; set; }
        public Activity MainWorkActivity { get; set; }
        public Activity MainNonWorkActivity { get; set; }
        public TimeSpan LenghtOfAllDayActivity { get; set; }
        public TimeSpan LenghtOfHalfDayActivity { get; set; }

        public ActivityGlobalSetting Clone()
        {
            return new ActivityGlobalSetting(this)
            {
                Id = this.Id,
            };
        }
    }
}
