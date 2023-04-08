using Attendance.WPF.Commands;
using Attendance.WPF.Models;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UserHistoryViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
		private readonly UserDailyOverviewViewModel _userDailyOverviewViewModel;
		private readonly AttendanceRecordStore _attendanceRecordStore;
        public UserHistoryViewModel(CurrentUserStore currentUser, AttendanceRecordStore attendanceRecordStore, UserDailyOverviewViewModel userDailyOverviewViewModel)
        {
            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;

            _userDailyOverviewViewModel = userDailyOverviewViewModel;
            UserDailyOverviewViewModel = userDailyOverviewViewModel;


            ChangeMonthCommand = new ChangeMonthCommand(this);

			Month = DateTime.Now.Month;
			Year = DateTime.Now.Year;
        }


		public UserDailyOverviewViewModel UserDailyOverviewViewModel { get; }
        public ICommand ChangeMonthCommand { get; }

		private int _year;
		public int Year
		{
			get
			{
				return _year;
			}
			set
			{
				_year = value;
				OnPropertyChanged(nameof(Year));
			}
		}

		private int _month;
		public int Month
		{
			get
			{
				return _month;
			}
			set
			{
				_month = value;
				OnPropertyChanged(nameof(Month));
                OnPropertyChanged(nameof(IsButtonNextMonthVisible));
                OnPropertyChanged(nameof(UserHistory));
            }
		}

		public List<MonthlyAttendanceTotalsWork> UserHistory => _attendanceRecordStore.MonthlyAttendanceTotalsWorks(_currentUser.User, Month, Year);

		public bool IsButtonNextMonthVisible => !(Year == DateTime.Now.Year && Month == DateTime.Now.Month);

		private int _selectedIndex = -1;
		public int SelectedIndex
        {
			get
			{
				return _selectedIndex;
			}
			set
			{
				_selectedIndex = value;
				OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(IsHistorySelected));
                OnPropertyChanged(nameof(SelectedHistory));

				_userDailyOverviewViewModel.Date = (IsHistorySelected) ? SelectedHistory.Date : DateOnly.FromDateTime(DateTime.Now);
            }
		}

		public bool IsHistorySelected => SelectedIndex != -1;

		public MonthlyAttendanceTotalsWork SelectedHistory => IsHistorySelected ? UserHistory[SelectedIndex] : null;

		public DateOnly SelectedDate => SelectedHistory.Date;
    }
}
