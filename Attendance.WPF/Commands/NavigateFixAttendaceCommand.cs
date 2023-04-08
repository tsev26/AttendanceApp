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
        private readonly SelectedUserStore _selectedUserStore;
        private readonly INavigationService _navigateFixAttendance;
        private readonly INavigationService _navigateFixesAttendance;
        private readonly UserDailyOverviewViewModel _userDailyOverviewViewModel;
        private readonly UserFixesAttendanceRecordViewModel _userFixesAttendanceRecordViewModel;

        public NavigateFixAttendaceCommand(SelectedUserStore selectedUserStore, UserDailyOverviewViewModel userDailyOverviewViewModel, INavigationService navigateFixAttendance, INavigationService navigateFixesAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _navigateFixAttendance = navigateFixAttendance;
            _navigateFixesAttendance = navigateFixesAttendance;
            _userDailyOverviewViewModel = userDailyOverviewViewModel;
        }

        public NavigateFixAttendaceCommand(SelectedUserStore selectedUserStore, UserFixesAttendanceRecordViewModel userFixesAttendanceRecordViewModel, INavigationService navigateFixAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _navigateFixAttendance = navigateFixAttendance;
            _userFixesAttendanceRecordViewModel = userFixesAttendanceRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            AttendanceRecord? attendanceRecord = null;
            bool isSelected = false;
            if (_userDailyOverviewViewModel != null)
            {
                attendanceRecord = _userDailyOverviewViewModel.CurrentUser.AttendanceRecordStore.AttendanceRecords.FirstOrDefault(a => a == _userDailyOverviewViewModel.SelectedAttendanceRecord?.AttendanceRecord);
                _selectedUserStore.SelectedUser = _userDailyOverviewViewModel.CurrentUser.User;
                isSelected = _userDailyOverviewViewModel.IsSelectedAttendanceRecord;
            }
            else if (_userFixesAttendanceRecordViewModel != null)
            {
                attendanceRecord = _userFixesAttendanceRecordViewModel.CurrentUser.AttendanceRecordStore.AttendanceRecords.FirstOrDefault(a => a == _userFixesAttendanceRecordViewModel.SelectedAttendanceRecord);
                _selectedUserStore.SelectedUser = _userFixesAttendanceRecordViewModel.CurrentUser.User;
                isSelected = _userFixesAttendanceRecordViewModel.IsSelectedAttendanceRecord;
            }

            
            if (parameter is string value)
            {
                if (value == "removeRecord" && isSelected && attendanceRecord != null)
                {
                    _selectedUserStore.AttendanceRecordStore.AddAttendanceRecordFixDelete(attendanceRecord);
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
