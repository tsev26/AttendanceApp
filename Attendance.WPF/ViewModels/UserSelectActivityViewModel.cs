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
    public class UserSelectActivityViewModel : ViewModelBase
    {
        private readonly ActivityStore _activityStore;
        private readonly CurrentUserStore _currentUserStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly INavigationService _navigateToHome;

        public List<Activity> Activities => _currentUserStore.User?.UserObligation.AvailableActivities.Where(a => !a.Property.IsPlan).ToList() ?? null;

        public UserSelectActivityViewModel(ActivityStore activityStore, 
                                           CurrentUserStore currentUserStore, 
                                           SelectedDataStore selectedUserStore,
                                           AttendanceRecordStore attendanceRecordStore,
                                           MessageStore messageStore,
                                           INavigationService navigateToHome)
        {
            _activityStore = activityStore;
            _currentUserStore = currentUserStore;
            _attendanceRecordStore = attendanceRecordStore;
            _navigateToHome = navigateToHome;


            UserSetActivityCommand = new UserSetActivityCommand(currentUserStore, selectedUserStore, attendanceRecordStore, messageStore, navigateToHome);
        }

        public ICommand UserSetActivityCommand { get; set; }

        private string _findSymbol;
        public string FindSymbol
        {
            get
            {
                return _findSymbol;
            }
            set
            {
                string code = value;
                if (code.Length == 2)
                {
                    code = code[1].ToString();
                }
                _findSymbol = code;
                ActivityExits();
                _findSymbol = "";
                OnPropertyChanged(nameof(FindSymbol));
            }
        }

        public string AfterLoad
        {
            get
            {
                CheckFastWorkSetting();
                return "";
            }
        }

        private async Task CheckFastWorkSetting()
        {
            Activity? currentUserActivity = _attendanceRecordStore.CurrentAttendanceRecord?.Activity ?? null;

            if (currentUserActivity != null && _currentUserStore.User.IsFastWorkSet && (!currentUserActivity.Property.Count || (currentUserActivity.Property.IsPause && !currentUserActivity.Property.IsPlan)))
            {
                Activity mainWorkActivity = _activityStore.GlobalSetting.MainWorkActivity;
                _attendanceRecordStore.AddAttendanceRecord(_currentUserStore.User, mainWorkActivity);
                _navigateToHome.Navigate();
            }
        }

        public void ActivityExits()
        {
            Activity activity = Activities.FirstOrDefault(a => a.Shortcut == FindSymbol);
            if (activity != null)
            {
                UserSetActivityCommand.Execute(activity);
            }
        }

        public override void Dispose()
        {
            UserSetActivityCommand = null;
            base.Dispose();
        }

    }
}
