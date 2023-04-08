using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Attendance.WPF.ViewModels
{
    public class UserDailyOverviewViewModel : ViewModelBase
    {
        private DispatcherTimer _timer;
        private readonly SelectedDataStore _selectedDataStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;

        public CurrentUserStore CurrentUser { get; }

        public UserDailyOverviewViewModel(CurrentUserStore currentUser, 
                                          SelectedDataStore selectedDataStore, 
                                          AttendanceRecordStore attendanceRecordStore,
                                          INavigationService navigateFixAttendance,
                                          INavigationService navigateFixesAttendance
                                          )
        {
            CurrentUser = currentUser;
            _selectedDataStore = selectedDataStore;
            _attendanceRecordStore = attendanceRecordStore;
            _selectedDataStore.AttendanceRecord = null;
            Date = DateOnly.FromDateTime(DateTime.Now);
            NavigateFixAttendaceCommand = new NavigateFixAttendaceCommand(selectedDataStore, this, attendanceRecordStore, navigateFixAttendance, navigateFixesAttendance);
            _attendanceRecordStore.CurrentAttendanceChange += CurrentUser_CurrentAttendanceChange;
        }

        public ICommand NavigateFixAttendaceCommand { get; }

        private void CurrentUser_CurrentAttendanceChange()
        {
            OnPropertyChanged(nameof(AttendanceRecordsInDay));
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

        public string WorkedInDayTotal => _attendanceRecordStore.WorkedInDayTotal(CurrentUser.User, Date);

        public string WorkedInDay => _attendanceRecordStore.WorkedInDay(CurrentUser.User, Date);

        public string PauseInDay => _attendanceRecordStore.PauseInDay(CurrentUser.User, Date);

        public List<AttendanceRecordItem> AttendanceRecordsInDay => _attendanceRecordStore.RecordsInDay(CurrentUser.User, Date);

        public List<AttendanceTotal> ActivitiesTotalInDay => _attendanceRecordStore.ActivitiesTotalInDay(CurrentUser.User, Date);


        private int _selectedAttendanceRecordIndex = -1;
        public int SelectedAttendanceRecordIndex
        {
            get
            {
                return _selectedAttendanceRecordIndex;
            }
            set
            {
                _selectedAttendanceRecordIndex = value;
                OnPropertyChanged(nameof(SelectedAttendanceRecordIndex));
                OnPropertyChanged(nameof(IsSelectedAttendanceRecord));
                OnPropertyChanged(nameof(SelectedAttendanceRecord));
            }
        }

        public bool IsSelectedAttendanceRecord => SelectedAttendanceRecordIndex != -1;

        public AttendanceRecordItem SelectedAttendanceRecord => IsSelectedAttendanceRecord ? AttendanceRecordsInDay[SelectedAttendanceRecordIndex] : null;

        public override void Dispose()
        {
            _attendanceRecordStore.CurrentAttendanceChange -= CurrentUser_CurrentAttendanceChange;
            StopClock();
            base.Dispose();
        }
    }
}
