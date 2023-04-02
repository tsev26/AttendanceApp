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
        private readonly SelectedUserStore _selectedUserStore;
        private readonly CurrentUser _currentUser;

        public UserSelectActivitySpecialViewModel(CurrentUser currentUser,
                                                  SelectedUserStore selectedUserStore,
                                                  ActivityStore activityStore,
                                                  INavigationService navigateHomeService,
                                                  INavigationService closeModalNavigationService)
        {
            _selectedUserStore = selectedUserStore;
            _currentUser = currentUser;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            UserSetActivityCommand = new UserSetActivityCommand(currentUser, activityStore, this, navigateHomeService, closeModalNavigationService);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand UserSetActivityCommand { get; }

        public List<int> Hours => Enumerable.Range(6, 17).ToList();
        public List<int> Minutes => new List<int> { 0, 15, 30, 45 };

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

        private DateTime _startActivity = DateTime.Now.Date;
        public DateTime StartActivity
        {
            get
            {
                return _startActivity;
            }
            set
            {
                _startActivity = value;
                OnPropertyChanged(nameof(StartActivity));
                if (StartActivity > EndActivity)
                {
                    EndActivity = StartActivity;
                    OnPropertyChanged(nameof(EndActivity));
                }
            }
        }

        private DateTime _endActivity = DateTime.Now.Date;
        public DateTime EndActivity
        {
            get
            {
                return _endActivity;
            }
            set
            {
                _endActivity = value;
                OnPropertyChanged(nameof(EndActivity));
                if (EndActivity < StartActivity)
                {
                    StartActivity = EndActivity;
                    OnPropertyChanged(nameof(StartActivity));
                }
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
            }
        }
    }
}
