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

        public List<Activity> Activities => _currentUserStore.User.UserObligation.AvailableActivities.ToList();

        public UserSelectActivityViewModel(ActivityStore activityStore, 
                                           CurrentUserStore currentUserStore, 
                                           SelectedDataStore selectedUserStore,
                                           AttendanceRecordStore attendanceRecordStore,
                                           INavigationService navigateToHome, 
                                           INavigationService navigateSpecialActivity)
        {
            _activityStore = activityStore;
            _currentUserStore = currentUserStore;
            UserSetActivityCommand = new UserSetActivityCommand(currentUserStore, selectedUserStore, attendanceRecordStore, navigateToHome, navigateSpecialActivity);
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
