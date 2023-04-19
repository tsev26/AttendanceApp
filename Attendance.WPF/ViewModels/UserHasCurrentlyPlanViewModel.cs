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
        private readonly CurrentUserStore _currentUser;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        public UserHasCurrentlyPlanViewModel(CurrentUserStore currentUser, 
                                             ActivityStore activityStore,
                                             AttendanceRecordStore attendanceRecordStore,
                                             MessageStore messageStore,
                                             INavigationService navigateHomeService,
                                             INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;

            UserSetActivityCommand = new UserSetActivityCommand(currentUser, activityStore, attendanceRecordStore, messageStore, navigateHomeService, closeModalNavigationService);
        }

        public AttendanceRecord CurrentAttendanceRecord => _attendanceRecordStore.CurrentAttendanceRecord;
        public ICommand UserSetActivityCommand { get; }
    }
}
