using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class ActivityViewShowsCommand : CommandBase
    {
        private ActivitiesViewModel _activitiesViewModel;

        public ActivityViewShowsCommand(ActivitiesViewModel activitiesViewModel)
        {
            _activitiesViewModel = activitiesViewModel;
        }

        public override void Execute(object? parameter)
        {
            _activitiesViewModel.SelectedActivityIndex = -1;
        }
    }
}
