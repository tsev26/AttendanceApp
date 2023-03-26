using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveActivityChangesCommand : CommandBase
    {
        private ActivityStore _activityStore;
        private ActivitiesViewModel _activitiesViewModel;

        public SaveActivityChangesCommand(ActivityStore activityStore, ActivitiesViewModel activitiesViewModel)
        {
            _activityStore = activityStore;
            _activitiesViewModel = activitiesViewModel;
        }

        public override void Execute(object? parameter)
        {
            _activityStore.UpdateActivity(_activitiesViewModel.SelectedActivity);
        }
    }
}
