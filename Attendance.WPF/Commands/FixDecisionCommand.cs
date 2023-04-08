﻿using Attendance.Domain.Models;
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
        private readonly UsersRequestsViewModel _usersRequestsViewModel;

        public FixDecisionCommand(AttendanceRecordStore attendanceRecordStore, UsersRequestsViewModel usersRequestsViewModel)
        {
            _attendanceRecordStore = attendanceRecordStore;
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
                }
                else if (value == "Reject")
                {
                    _attendanceRecordStore.RejectFix(attendanceRecordFix);
                }
            }
        }
    }
}
