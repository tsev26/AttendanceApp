using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UserSelectActivitySpecialViewModel : ViewModelBase
    {
        private readonly SelectedDataStore _selectedUserStore;
        private readonly CurrentUserStore _currentUser;
        public UserSelectActivitySpecialViewModel(CurrentUserStore currentUser,
                                                  SelectedDataStore selectedUserStore,
                                                  ActivityStore activityStore,
                                                  AttendanceRecordStore attendanceRecordStore,
                                                  MessageStore messageStore,
                                                  INavigationService navigateHomeService,
                                                  INavigationService closeModalNavigationService)
        {
            _selectedUserStore = selectedUserStore;
            _currentUser = currentUser;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            UserSetActivityCommand = new UserSetActivityCommand(currentUser, activityStore, attendanceRecordStore, this, messageStore, navigateHomeService, closeModalNavigationService);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand UserSetActivityCommand { get; }


        public List<int> StartHours => (StartActivity.Date == DateTime.Now.Date) ? Enumerable.Range(DateTime.Now.Hour, 24 - DateTime.Now.Hour).ToList() : Enumerable.Range(6, 17).ToList();

        public List<int> EndHours => (EndActivity.Date == DateTime.Now.Date) ? Enumerable.Range(DateTime.Now.Hour, 24-DateTime.Now.Hour).ToList() : Enumerable.Range(6, 17).ToList();

        public List<int> StartMinutes => (StartActivity.Date == DateTime.Now.Date) ? Enumerable.Range(DateTime.Now.Minute, 59-DateTime.Now.Minute).Select(n => ((int)(n / 15.0)) * 15).Distinct().ToList() : Enumerable.Range(0, 59).Select(n => ((int)(n / 15.0)) * 15).Distinct().ToList();
        public List<int> EndMinutes => (StartActivity.Date == DateTime.Now.Date && StartHour == EndHour) ? Enumerable.Range(DateTime.Now.Minute, 59-DateTime.Now.Minute).Select(n => ((int)(n / 15.0)) * 15).Distinct().ToList() : Enumerable.Range(0, 59).Select(n => ((int)(n / 15.0)) * 15).Distinct().ToList();


        public int DescriptionWidth => SelectedActivity.Property.HasTime ? 300 : 295;
        public Activity SelectedActivity => _selectedUserStore.SelectedActivity;

        public bool HasNoTime => !SelectedActivity.Property.HasTime;

        private bool _isFullDayPlan = true;
        public bool IsFullDayPlan
        {
            get
            {
                return _isFullDayPlan;
            }
            set
            {
                _isFullDayPlan = value;
                OnPropertyChanged(nameof(IsFullDayPlan));
                OnPropertyChanged(nameof(IsHalfDayPlan));
            }
        }

        public bool IsHalfDayPlan => !IsFullDayPlan;

        private bool _isHalfDayStart;
        public bool IsHalfDayStart
        {
            get
            {
                return _isHalfDayStart;
            }
            set
            {
                _isHalfDayStart = value;
                OnPropertyChanged(nameof(IsHalfDayStart));
                if (IsHalfDayEnd == IsHalfDayStart)
                {
                    IsHalfDayEnd = !IsHalfDayStart;
                    OnPropertyChanged(nameof(IsHalfDayEnd));
                }
            }
        }

        private bool _isHalfDayEnd = true;
        public bool IsHalfDayEnd
        {
            get
            {
                return _isHalfDayEnd;
            }
            set
            {
                _isHalfDayEnd = value;
                OnPropertyChanged(nameof(IsHalfDayEnd));
                if (IsHalfDayEnd == IsHalfDayStart)
                {
                    IsHalfDayStart = !IsHalfDayEnd;
                    OnPropertyChanged(nameof(IsHalfDayStart));
                }
            }
        }

        private DateTime _startActivity = DateTime.Now.Date.AddDays(1);
        public DateTime StartActivity
        {
            get
            {
                return _startActivity;
            }
            set
            {
                _startActivity = value;
                if (_startActivity < DateTime.Now.Date)
                {
                    _startActivity = DateTime.Now.Date;
                }
                OnPropertyChanged(nameof(StartActivity));
                if (StartActivity > EndActivity)
                {
                    EndActivity = StartActivity;
                    OnPropertyChanged(nameof(EndActivity));
                }
                OnPropertyChanged(nameof(StartMinutes));
                OnPropertyChanged(nameof(StartHours));
                StartHour = StartHours[0];
                StartMinute = StartMinutes[0];
            }
        }

        private DateTime _endActivity = DateTime.Now.Date.AddDays(1);
        public DateTime EndActivity
        {
            get
            {
                return _endActivity;
            }
            set
            {
                _endActivity = value;
                if (_endActivity < DateTime.Now.Date)
                {
                    _endActivity = DateTime.Now.Date;
                }
                OnPropertyChanged(nameof(EndActivity));
                if (EndActivity < StartActivity)
                {
                    StartActivity = EndActivity;
                    OnPropertyChanged(nameof(StartActivity));
                }
                OnPropertyChanged(nameof(EndHours));
                OnPropertyChanged(nameof(EndMinute));
                EndHour = EndHours[0];
                EndMinute = EndMinutes[0];
            }
        }



        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private int _startHour = 8;
        public int StartHour
        {
            get
            {
                return _startHour;
            }
            set
            {
                _startHour = value;
                OnPropertyChanged(nameof(StartHour));
                if (StartHour > EndHour && StartActivity.Date == EndActivity.Date)
                {
                    EndHour = StartHour;
                    OnPropertyChanged(nameof(EndHour));
                }
                OnPropertyChanged(nameof(StartMinutes));
            }
        }

        private int _startMinute = 0;
        public int StartMinute
        {
            get
            {
                return _startMinute;
            }
            set
            {
                _startMinute = value;
                OnPropertyChanged(nameof(StartMinute));
                if (StartMinute > EndMinute && StartActivity.Date == EndActivity.Date && StartHour == EndHour)
                {
                    EndMinute = StartMinute;
                    OnPropertyChanged(nameof(EndMinute));
                }
            }
        }

        private int _endHour = 16;
        public int EndHour
        {
            get
            {
                return _endHour;
            }
            set
            {
                _endHour = value;
                OnPropertyChanged(nameof(EndHour));
                if (EndHour < StartHour && StartActivity.Date == EndActivity.Date)
                {
                    StartHour = EndHour;
                    OnPropertyChanged(nameof(StartHour));
                }
                OnPropertyChanged(nameof(EndMinutes));
            }
        }

        private int _endMinute = 0;
        public int EndMinute
        {
            get
            {
                return _endMinute;
            }
            set
            {
                _endMinute = value;
                OnPropertyChanged(nameof(EndMinute));
                if (EndMinute < StartMinute && StartActivity.Date == EndActivity.Date && StartHour == EndHour)
                {
                    StartMinute = EndMinute;
                    OnPropertyChanged(nameof(StartMinute));
                }
            }
        }
    }
}
