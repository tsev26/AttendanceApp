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

        public override void Dispose()
        {
            UserDailyOverviewViewModel.Dispose();
            UserSelectActivityViewModel.Dispose();
            base.Dispose();
        }
    }
}
