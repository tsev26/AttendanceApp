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
    public class SaveAttendanceRecordChangesCommand : CommandBase
    {
        private readonly SelectedDataStore _selectedUserStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly UserFixAttendanceRecordViewModel _userFixAttendanceRecordViewModel;
        private readonly INavigationService _closeModalNavigationService;
        private readonly INavigationService _navigateFixesAttendance;

        public SaveAttendanceRecordChangesCommand(SelectedDataStore selectedUserStore, 
                                                  AttendanceRecordStore attendanceRecordStore,
                                                  UserFixAttendanceRecordViewModel userFixAttendanceRecordViewModel, 
                                                  INavigationService closeModalNavigationService,
                                                  INavigationService navigateFixesAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _attendanceRecordStore = attendanceRecordStore;
            _userFixAttendanceRecordViewModel = userFixAttendanceRecordViewModel;
            _closeModalNavigationService = closeModalNavigationService;
            _navigateFixesAttendance = navigateFixesAttendance;
        }

        public override void Execute(object? parameter)
        {
            //add entry 
            if (_selectedUserStore.AttendanceRecord == null)
            {
                DateTime newEntry = new DateTime(_userFixAttendanceRecordViewModel.Date.Year, _userFixAttendanceRecordViewModel.Date.Month, _userFixAttendanceRecordViewModel.Date.Day, _userFixAttendanceRecordViewModel.Hour, _userFixAttendanceRecordViewModel.Minute, 0);
                _attendanceRecordStore.AddAttendanceRecordFixInsert(_selectedUserStore.SelectedUser,_userFixAttendanceRecordViewModel.Activity, newEntry);
            }
            //edit entry
            else
            {
                DateTime newEntry = new DateTime(_userFixAttendanceRecordViewModel.Date.Year, _userFixAttendanceRecordViewModel.Date.Month, _userFixAttendanceRecordViewModel.Date.Day, _userFixAttendanceRecordViewModel.Hour, _userFixAttendanceRecordViewModel.Minute, 0);
                _attendanceRecordStore.AddAttendanceRecordFixUpdate(_selectedUserStore.SelectedUser, _selectedUserStore.AttendanceRecord, _userFixAttendanceRecordViewModel.Activity, newEntry);
            }

            _closeModalNavigationService.Navigate();
            _navigateFixesAttendance.Navigate();
        }
    }
}
