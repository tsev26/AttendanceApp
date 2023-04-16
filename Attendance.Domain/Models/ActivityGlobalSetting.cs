using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class ActivityGlobalSetting : DomainObject
    {
        public ActivityGlobalSetting() : base() { }
        public ActivityGlobalSetting(ActivityGlobalSetting activityGlobalSetting) : base()
        {
            PauseEvery = activityGlobalSetting.PauseEvery;
            PauseDuration = activityGlobalSetting.PauseDuration;
            MainWorkActivity = activityGlobalSetting.MainWorkActivity;
            MainPauseActivity = activityGlobalSetting.MainPauseActivity;
            MainNonWorkActivity = activityGlobalSetting.MainNonWorkActivity;
            LenghtOfAllDayActivity = activityGlobalSetting.LenghtOfAllDayActivity;
            LenghtOfHalfDayActivity = activityGlobalSetting.LenghtOfHalfDayActivity;
        }

        public ActivityGlobalSetting(TimeSpan pauseEvery, 
                                     TimeSpan pauseDuration, 
                                     Activity mainWorkActivity, 
                                     Activity mainPauseActivity,
                                     Activity mainNonWorkActivity, 
                                     TimeSpan lenghtOfAllDayActivity, 
                                     TimeSpan lenghtOfHalfDayActivity) : base()
        {
            PauseEvery = pauseEvery;
            PauseDuration = pauseDuration;
            MainWorkActivity = mainWorkActivity;
            MainPauseActivity = mainPauseActivity;
            MainNonWorkActivity = mainNonWorkActivity;
            LenghtOfAllDayActivity = lenghtOfAllDayActivity;
            LenghtOfHalfDayActivity = lenghtOfHalfDayActivity;
        }

        public TimeSpan PauseEvery { get; set; }
        public TimeSpan PauseDuration { get; set; }

        [ForeignKey("MainWorkActivityId")]
        public Activity MainWorkActivity { get; set; }
        public int MainWorkActivityId { get; set; }

        [ForeignKey("MainPauseActivityId")]
        public Activity MainPauseActivity { get; set; }
        public int MainPauseActivityId { get; set; }

        [ForeignKey("MainNonWorkActivityId")]
        public Activity MainNonWorkActivity { get; set; }
        public int MainNonWorkActivityId { get; set; }
        public TimeSpan LenghtOfAllDayActivity { get; set; }
        public TimeSpan LenghtOfHalfDayActivity { get; set; }

        public ActivityGlobalSetting Clone()
        {
            return new ActivityGlobalSetting(this)
            {
                ID = this.ID,
            };
        }
    }
}
