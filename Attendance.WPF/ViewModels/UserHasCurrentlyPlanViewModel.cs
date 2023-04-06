using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UserHasCurrentlyPlanViewModel : ViewModelBase
    {
        private readonly CurrentUser _currentUser;

        public UserHasCurrentlyPlanViewModel(CurrentUser currentUser, 
                                             ActivityStore activityStore,
                                             INavigationService navigateHomeService,
                                             INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            UserSetActivityCommand = new UserSetActivityCommand(currentUser, activityStore, navigateHomeService, closeModalNavigationService);
        }

        public AttendanceRecord CurrentAttendanceRecord => _currentUser.AttendanceRecordStore.CurrentAttendanceRecord;
        public ICommand UserSetActivityCommand { get; }
    }
}
