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
    public class DeleteAttendanceRecordFixCommand : CommandBase
    {
        private readonly CurrentUser _currentUser;
        private readonly UserFixesAttendanceRecordViewModel _userFixesAttendanceRecordViewModel;

        public DeleteAttendanceRecordFixCommand(CurrentUser currentUser, UserFixesAttendanceRecordViewModel userFixesAttendanceRecordViewModel)
        {
            _currentUser = currentUser;
            _userFixesAttendanceRecordViewModel = userFixesAttendanceRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _currentUser.AttendanceRecordStore.RemoveAttendanceRecordFix(_userFixesAttendanceRecordViewModel.SelectedAttendanceRecordFix);
        }
    }
}
