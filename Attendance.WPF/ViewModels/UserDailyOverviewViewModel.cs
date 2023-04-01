using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += (s, e) => OnTimeChanged();
            _timer.Start();
        }

        private void StopClock()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Tick -= (s, e) => OnTimeChanged();
            _timer = null;
        }

        private DateOnly _date;
        

        public DateOnly Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
                if (Date == DateOnly.FromDateTime(DateTime.Now))
                {
                    StartClock();
                }
                else
                {
                    StopClock();
                }
                OnTimeChanged();
                OnPropertyChanged(nameof(AttendanceRecordsInDay));
                OnPropertyChanged(nameof(DateName));
            }
        }

        public string DateName => CultureInfo.GetCultureInfo("cs-CZ").DateTimeFormat.GetDayName(Date.DayOfWeek);

        private void OnTimeChanged()
        {
            OnPropertyChanged(nameof(WorkedInDayTotal));
            OnPropertyChanged(nameof(WorkedInDay));
            OnPropertyChanged(nameof(PauseInDay));
            OnPropertyChanged(nameof(ActivitiesTotalInDay));
        }

        public string WorkedInDayTotal => CurrentUser.WorkedInDayTotal(Date);

        public string WorkedInDay => CurrentUser.WorkedInDay(Date);

        public string PauseInDay => CurrentUser.PauseInDay(Date);

        public List<AttendanceRecord> AttendanceRecordsInDay => CurrentUser.RecordsInDay(Date);

        public List<AttendanceTotal> ActivitiesTotalInDay => CurrentUser.ActivitiesTotalInDay(Date);


        public override void Dispose()
        {
            StopClock();
            base.Dispose();
        }
    }
}
