using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Attendance.WPF.ViewModels
{
    public class UserDailyOverviewViewModel : ViewModelBase
    {
        private DispatcherTimer _timer;
        public CurrentUser CurrentUser { get; }

        public UserDailyOverviewViewModel(CurrentUser currentUser)
        {
            CurrentUser = currentUser;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            _timer.Start();
        }

        private void OnTimeChanged(DateTime now)
        {
            OnPropertyChanged(nameof(WorkedInDayTotal));
            OnPropertyChanged(nameof(WorkedInDay));
            OnPropertyChanged(nameof(PauseInDay));
            OnPropertyChanged(nameof(ActivitiesTotalInDay));
        }

        public string WorkedInDayTotal => CurrentUser.WorkedInDayTotal(DateOnly.FromDateTime(DateTime.Now));

        public string WorkedInDay => CurrentUser.WorkedInDay(DateOnly.FromDateTime(DateTime.Now));

        public string PauseInDay => CurrentUser.PauseInDay(DateOnly.FromDateTime(DateTime.Now));

        public List<AttendanceRecord> AttendanceRecordsInDay => CurrentUser.RecordsInDay(DateOnly.FromDateTime(DateTime.Now));

        public List<AttendanceTotal> ActivitiesTotalInDay => CurrentUser.ActivitiesTotalInDay(DateOnly.FromDateTime(DateTime.Now));


        public override void Dispose()
        {
            _timer.Stop();
            base.Dispose();
        }
    }
}
