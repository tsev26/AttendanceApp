using Attendance.Domain.Models;
using Attendance.EF.Services;
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
        private readonly ActivityDataService _activityDataService;

        public ActivityStore(ActivityDataService activityDataService)
        {
            _activities = new List<Activity>();
            _activityDataService = activityDataService; 
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
                ActivitiesChange?.Invoke();
            }
        }

        public async Task AddActivity(Activity activity)
        {
            _activityDataService.AddActivity(activity);
            _activities.Add(activity);
            ActivitiesChange?.Invoke();
        }

        public void UpdateActivity(Activity activity)
        {
            _activityDataService.UpdateActivity(activity);
            int index = _activities.FindIndex(a => a.ID == activity.ID);
            if (index != -1)
            {
                _activities[index] = activity;
            }

            ActivitiesChange?.Invoke();
        }

        public void UpdateActivityGlobalSetting(ActivityGlobalSetting activityGlobalSetting)
        {
            _activityDataService.UpdateGlobalSetting(activityGlobalSetting);
            _globalSetting = activityGlobalSetting;
            GlobalSettingChange?.Invoke();
        }

        public async Task LoadActivities()
        {
            Activities = await _activityDataService.GetActivities();
        }
    }
}
