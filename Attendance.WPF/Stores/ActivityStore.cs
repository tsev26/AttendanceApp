using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class ActivityStore
    {
        private List<Activity> _activities;
        private ActivityGlobalSetting _globalSetting;

        public ActivityStore()
        {
            _activities = new List<Activity>();
        }

        public event Action ActivitiesChange;
        public event Action GlobalSettingChange;

        public ActivityGlobalSetting GlobalSetting
        {
            get { return _globalSetting; }
            set
            {
                _globalSetting = value;
            }
        }

        public List<Activity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
            }
        }

        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
            ActivitiesChange?.Invoke();
        }

        public void UpdateActivity(Activity activity)
        {
            int index = _activities.FindIndex(a => a.Id == activity.Id);
            if (index != -1)
            {
                _activities[index] = activity;
            }
            ActivitiesChange?.Invoke();
        }

        public void UpdateActivityGlobalSetting(ActivityGlobalSetting activityGlobalSetting)
        {
            _globalSetting = activityGlobalSetting;
            GlobalSettingChange?.Invoke();
        }
    }
}
