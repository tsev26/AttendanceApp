using Attendance.Domain.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserMenuViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly ActivityStore _activityStore;
        private readonly INavigationService _navigateHome;
        public UserMenuViewModel(UserSelectActivityViewModel userSelectActivityViewModel,
                                 UserDailyOverviewViewModel userDailyOverviewViewModel,
                                 CurrentUserStore currentUser,
                                 AttendanceRecordStore attendanceRecordStore,
                                 INavigationService navigateUserHasCurretlyPlanService)
        {
            UserDailyOverviewViewModel = userDailyOverviewViewModel;
            UserSelectActivityViewModel = userSelectActivityViewModel;

            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;


            if (_attendanceRecordStore.CurrentAttendanceRecord(_currentUser.User)?.Activity.Property.IsPlan ?? false)
            {
                navigateUserHasCurretlyPlanService.Navigate();
            }
        }

        public UserDailyOverviewViewModel UserDailyOverviewViewModel { get; }
        public UserSelectActivityViewModel UserSelectActivityViewModel { get; }

        private void CheckFastWorkSetting()
        {
            Activity? currentUserActivity = _attendanceRecordStore.CurrentAttendanceRecord(_currentUser.User).Activity;

            if (currentUserActivity != null && _currentUser.User.IsFastWorkSet && (!currentUserActivity.Property.Count || (currentUserActivity.Property.IsPause && !currentUserActivity.Property.IsPlan)))
            {
                Activity mainWorkActivity = _activityStore.GlobalSetting.MainWorkActivity;
                _attendanceRecordStore.AddAttendanceRecord(_currentUser.User, mainWorkActivity);
                _navigateHome.Navigate();
            }
        }

        public override void Dispose()
        {
            UserDailyOverviewViewModel.Dispose();
            UserSelectActivityViewModel.Dispose();
            base.Dispose();
        }
    }
}
