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
    public class UserPlanViewModel : ViewModelBase
    {
        private readonly ActivityStore _activityStore;
        private readonly CurrentUserStore _currentUserStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly INavigationService _navigateToHome;

        public UserPlanViewModel(ActivityStore activityStore, 
                                 CurrentUserStore currentUserStore,
                                 SelectedDataStore selectedUserStore,
                                 AttendanceRecordStore attendanceRecordStore,
                                 MessageStore messageStore,
                                 INavigationService navigateToHome,
                                 INavigationService navigateSpecialActivity)
        {
            _activityStore = activityStore;
            _currentUserStore = currentUserStore;
            _attendanceRecordStore = attendanceRecordStore;
            _navigateToHome = navigateToHome;

            _attendanceRecordStore.CurrentAttendanceChange += AttendanceRecordStore_CurrentAttendanceChange;

            UserSetActivityCommand = new UserSetActivityCommand(currentUserStore, selectedUserStore, attendanceRecordStore, this, messageStore, navigateToHome, navigateSpecialActivity);

        }

        private void AttendanceRecordStore_CurrentAttendanceChange()
        {
            OnPropertyChanged(nameof(FuturePlans));
        }

        public ICommand UserSetActivityCommand { get; set; }


        public List<Activity> Activities => _currentUserStore.User?.UserObligation.AvailableActivities.Where(a => a.Property.IsPlan).ToList() ?? null;

        public List<AttendanceRecord> FuturePlans => _attendanceRecordStore.AttendanceRecords(_currentUserStore.User).Where(a => a.Entry > DateTime.Now && a.AttendanceRecordDetail != null).ToList();

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
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(SelectedFuturePlan));
            }
        }

        public bool IsSelected => SelectedIndex != -1;
        public AttendanceRecord SelectedFuturePlan => IsSelected ? FuturePlans[SelectedIndex] : null;


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
            _attendanceRecordStore.CurrentAttendanceChange -= AttendanceRecordStore_CurrentAttendanceChange;
            base.Dispose();
        }
    }
}
