using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class FixDecisionCommand : CommandBase
    {
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly MessageStore _messageStore;
        private readonly UsersRequestsViewModel _usersRequestsViewModel;

        public FixDecisionCommand(AttendanceRecordStore attendanceRecordStore, MessageStore messageStore, UsersRequestsViewModel usersRequestsViewModel)
        {
            _attendanceRecordStore = attendanceRecordStore;
            _messageStore = messageStore;
            _usersRequestsViewModel = usersRequestsViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                AttendanceRecordFix attendanceRecordFix = _usersRequestsViewModel.SelectedPendingRequestFix;
                if (value == "Approve")
                {
                    _attendanceRecordStore.ApproveFix(attendanceRecordFix);
                    _messageStore.Message = "Změna v docházce schválena";
                }
                else if (value == "Reject")
                {
                    _attendanceRecordStore.RejectFix(attendanceRecordFix);
                    _messageStore.Message = "Změna v docházce zamutnuta";
                }
            }
        }
    }
}
