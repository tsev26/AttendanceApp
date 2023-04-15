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
        private readonly CurrentUserStore _currentUser;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly MessageStore _messageStore;
        private readonly UserFixesAttendanceRecordViewModel _userFixesAttendanceRecordViewModel;

        public DeleteAttendanceRecordFixCommand(CurrentUserStore currentUser, 
                                                AttendanceRecordStore attendanceRecordStore, 
                                                MessageStore messageStore,
                                                UserFixesAttendanceRecordViewModel userFixesAttendanceRecordViewModel)
        {
            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;
            _messageStore = messageStore;
            _userFixesAttendanceRecordViewModel = userFixesAttendanceRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _attendanceRecordStore.RemoveAttendanceRecordFix(_userFixesAttendanceRecordViewModel.SelectedAttendanceRecordFix);
            _messageStore.Message = "Žádost o úpravu docházky odstraněna";
        }
    }
}
