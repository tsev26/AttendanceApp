using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class NavigateFixAttendaceCommand : CommandBase
    {
        private readonly SelectedUserStore _selectedUserStore;
        private readonly INavigationService _navigateFixAttendance;
        private readonly UserDailyOverviewViewModel _userDailyOverviewViewModel;

        public NavigateFixAttendaceCommand(SelectedUserStore selectedUserStore, INavigationService navigateFixAttendance, UserDailyOverviewViewModel userDailyOverviewViewModel)
        {
            _selectedUserStore = selectedUserStore;
            _navigateFixAttendance = navigateFixAttendance;
            _userDailyOverviewViewModel = userDailyOverviewViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_selectedUserStore.SelectedUser == null)
            {
                _selectedUserStore.SelectedUser = _userDailyOverviewViewModel.CurrentUser.User;
            }
            if (parameter is string value)
            {
                if (_userDailyOverviewViewModel.IsSelectedAttendanceRecord && value == "updateRecord")
                {
                    _selectedUserStore.AttendanceRecord = _userDailyOverviewViewModel.SelectedAttendanceRecord;
                }
            }
  
            _navigateFixAttendance.Navigate();

        }
    }
}
