using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserDailyOverviewViewModel : ViewModelBase
    {
        public CurrentUser CurrentUser { get; }

        public UserDailyOverviewViewModel(CurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public string WorkedInDayTotal => CurrentUser.WorkedInDayTotal(DateOnly.FromDateTime(DateTime.Now));

        public string WorkedInDay => CurrentUser.WorkedInDay(DateOnly.FromDateTime(DateTime.Now));

        public string PauseInDay => CurrentUser.PauseInDay(DateOnly.FromDateTime(DateTime.Now));

        public List<AttendanceRecord> AttendanceRecordsInDay => CurrentUser.RecordsInDay(DateOnly.FromDateTime(DateTime.Now));

        public List<AttendanceTotal> ActivitiesTotalInDay => CurrentUser.ActivitiesTotalInDay(DateOnly.FromDateTime(DateTime.Now));

    }
}
