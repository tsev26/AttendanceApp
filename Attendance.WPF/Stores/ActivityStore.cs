using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class ActivityStore
    {
        private List<Activity> _activities;

        public ActivityStore()
        {
            _activities = new List<Activity>();
        }

        public List<Activity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
            }
        }

        public void AddActivity(Activity newActivity)
        {
            _activities.Add(newActivity);
        }
    }
}
