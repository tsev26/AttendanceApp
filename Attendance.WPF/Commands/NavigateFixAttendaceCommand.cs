using Attendance.Domain.Models;
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
        private readonly SelectedDataStore _selectedUserStore;
        private readonly INavigationService _navigateFixAttendance;
        private readonly INavigationService _navigateFixesAttendance;
        private readonly UserDailyOverviewViewModel _userDailyOverviewViewModel;
        private readonly UserFixesAttendanceRecordViewModel _userFixesAttendanceRecordViewModel;
        private readonly AttendanceRecordStore _attendanceRecordStore;

        public NavigateFixAttendaceCommand(SelectedDataStore selectedUserStore, 
                                           UserDailyOverviewViewModel userDailyOverviewViewModel,
                                           AttendanceRecordStore attendanceRecordStore,
                                           INavigationService navigateFixAttendance, 
                                           INavigationService navigateFixesAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _navigateFixAttendance = navigateFixAttendance;
            _navigateFixesAttendance = navigateFixesAttendance;
            _userDailyOverviewViewModel = userDailyOverviewViewModel;
            _attendanceRecordStore = attendanceRecordStore;
        }

        public NavigateFixAttendaceCommand(SelectedDataStore selectedUserStore, 
                                           UserFixesAttendanceRecordViewModel userFixesAttendanceRecordViewModel,
                                           AttendanceRecordStore attendanceRecordStore,
                                           INavigationService navigateFixAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _navigateFixAttendance = navigateFixAttendance;
            _userFixesAttendanceRecordViewModel = userFixesAttendanceRecordViewModel;
            _attendanceRecordStore = attendanceRecordStore;
        }

        public override void Execute(object? parameter)
        {
            AttendanceRecord? attendanceRecord = null;
            User user = null;
            bool isSelected = false;
            if (_userDailyOverviewViewModel != null)
            {
                user = _userDailyOverviewViewModel.CurrentUser.User;
                attendanceRecord = _attendanceRecordStore.AttendanceRecords(user).FirstOrDefault(a => a == _userDailyOverviewViewModel.SelectedAttendanceRecord?.AttendanceRecord);
                _selectedUserStore.SelectedUser = user;
                isSelected = _userDailyOverviewViewModel.IsSelectedAttendanceRecord;
            }
            else if (_userFixesAttendanceRecordViewModel != null)
            {
                user = _userFixesAttendanceRecordViewModel.CurrentUser.User;
                attendanceRecord = _attendanceRecordStore.AttendanceRecords(user).FirstOrDefault(a => a == _userFixesAttendanceRecordViewModel.SelectedAttendanceRecord);
                _selectedUserStore.SelectedUser = user;
                isSelected = _userFixesAttendanceRecordViewModel.IsSelectedAttendanceRecord;
            }

            
            if (parameter is string value)
            {
                if (value == "removeRecord" && isSelected && attendanceRecord != null)
                {
                    _attendanceRecordStore.AddAttendanceRecordFixDelete(user,attendanceRecord);
                    _navigateFixesAttendance.Navigate();
                }
                else if (value == "updateRecord" && isSelected && attendanceRecord != null)
                {
                    _selectedUserStore.AttendanceRecord = attendanceRecord;
                    _navigateFixAttendance.Navigate();
                }
                else if (value == "addRecord")
                {
                    _navigateFixAttendance.Navigate();
                }
            }
        }
    }
}
