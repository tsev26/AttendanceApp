using Attendance.Domain.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.Commands
{
    public class CreateActivityCommand : CommandBase
    {
        private ActivityStore _activityStore;
        private ActivityUpsertViewModel _activityUpsertViewModel;
        private CloseModalNavigationService _closeModalNavigationService;

        public CreateActivityCommand(ActivityStore activityStore, ActivityUpsertViewModel activityUpsertViewModel, CloseModalNavigationService closeModalNavigationService)
        {
            _activityStore = activityStore;
            _activityUpsertViewModel = activityUpsertViewModel;
            _closeModalNavigationService = closeModalNavigationService;
        }

        public override void Execute(object? parameter)
        {
            ActivityProperty activityProperty = new ActivityProperty(_activityUpsertViewModel.IsPlan,
                                                                     _activityUpsertViewModel.Count,
                                                                     _activityUpsertViewModel.IsPause,
                                                                     _activityUpsertViewModel.HasPause,
                                                                     _activityUpsertViewModel.MaxInDay,
                                                                     _activityUpsertViewModel.GroupByName);

            Activity newActivity = new Activity(_activityUpsertViewModel.ActivityName, _activityUpsertViewModel.ActivityShortcut, activityProperty);
            _activityStore.AddActivity(newActivity);
            _closeModalNavigationService.Navigate();
        }
    }
}
