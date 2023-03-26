﻿using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class ActivityUpsertViewModel : ViewModelBase
    {
        public ActivityUpsertViewModel(ActivityStore activityStore, CloseModalNavigationService closeModalNavigationService)
        {
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            CreateActivityCommand = new CreateActivityCommand(activityStore, this, closeModalNavigationService);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand CreateActivityCommand { get; }


        private string _activityName;
        public string ActivityName
        {
            get
            {
                return _activityName;
            }
            set
            {
                _activityName = value;
                OnPropertyChanged(nameof(ActivityName));
            }
        }

        private string _activityShortcut;
        public string ActivityShortcut
        {
            get
            {
                return _activityShortcut;
            }
            set
            {
                _activityShortcut = value;
                OnPropertyChanged(nameof(ActivityShortcut));
            }
        }

        private bool _isPlan;
        public bool IsPlan
        {
            get
            {
                return _isPlan;
            }
            set
            {
                _isPlan = value;
                OnPropertyChanged(nameof(IsPlan));
            }
        }

        private bool _count;
        public bool Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        private bool _isPause;
        public bool IsPause
        {
            get
            {
                return _isPause;
            }
            set
            {
                _isPause = value;
                OnPropertyChanged(nameof(IsPause));
            }
        }

        private bool _hasPause;
        public bool HasPause
        {
            get
            {
                return _hasPause;
            }
            set
            {
                _hasPause = value;
                OnPropertyChanged(nameof(HasPause));
            }
        }

        private TimeSpan _pauseEvery;
        public TimeSpan PauseEvery
        {
            get
            {
                return _pauseEvery;
            }
            set
            {
                _pauseEvery = value;
                OnPropertyChanged(nameof(PauseEvery));
            }
        }

        private TimeSpan _pauseDuration;
        public TimeSpan PauseDuration
        {
            get
            {
                return _pauseDuration;
            }
            set
            {
                _pauseDuration = value;
                OnPropertyChanged(nameof(PauseDuration));
            }
        }


        private bool _hasExpectedStart;
        public bool HasExpectedStart
        {
            get
            {
                return _hasExpectedStart;
            }
            set
            {
                _hasExpectedStart = value;
                OnPropertyChanged(nameof(HasExpectedStart));
            }
        }

        private bool _hasExpectedReturn;
        public bool HasExpectedReturn
        {
            get
            {
                return _hasExpectedReturn;
            }
            set
            {
                _hasExpectedReturn = value;
                OnPropertyChanged(nameof(HasExpectedReturn));
            }
        }

        private bool _isFullDayActivity;
        public bool IsFullDayActivity
        {
            get
            {
                return _isFullDayActivity;
            }
            set
            {
                _isFullDayActivity = value;
                OnPropertyChanged(nameof(IsFullDayActivity));
            }
        }

        private bool _isHalfDayActivity;
        public bool IsHalfDayActivity
        {
            get
            {
                return _isHalfDayActivity;
            }
            set
            {
                _isHalfDayActivity = value;
                OnPropertyChanged(nameof(IsHalfDayActivity));
            }
        }

        private string _groupByName;
        public string GroupByName
        {
            get
            {
                return _groupByName;
            }
            set
            {
                _groupByName = value;
                OnPropertyChanged(nameof(GroupByName));
            }
        }
    }
}
