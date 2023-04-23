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
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
        private readonly AttendanceRecordStore _attendanceRecordStore;

        public NavigationBarViewModel(INavigationService navigateHomeService, 
                                      INavigationService navigateUsersKeysService,
                                      INavigationService navigateUserMenuService,
                                      INavigationService navigateUserProfileService,
                                      INavigationService navigateGroupsService,
                                      INavigationService navigationActivitiesService,
                                      INavigationService navigateUsersService,
                                      INavigationService navigateHistoryService,
                                      INavigationService navigateFixesService,
                                      INavigationService navigateRequestService,
                                      INavigationService navigateUserPlanService,
                                      CurrentUserStore currentUser,
                                      AttendanceRecordStore attendanceRecordStore,
                                      MessageStore messageStore)
        {
            NavigateHomeCommand = new NavigateCommand(navigateHomeService);
            NavigateUsersKeysCommand = new NavigateCommand(navigateUsersKeysService);
            NavigateUserMenuCommand = new NavigateCommand(navigateUserMenuService);
            NavigateProfileCommand = new NavigateCommand(navigateUserProfileService);
            NavigateGroupsCommand = new NavigateCommand(navigateGroupsService);
            NavigateActivitiesCommand = new NavigateCommand(navigationActivitiesService);
            NavigateUsersCommand = new NavigateCommand(navigateUsersService);
            NavigateHistoryCommnad = new NavigateCommand(navigateHistoryService);
            NavigateFixesCommnad = new NavigateCommand(navigateFixesService);
            NavigateRequestsCommand = new NavigateCommand(navigateRequestService);
            NavigateUserPlanCommand = new NavigateCommand(navigateUserPlanService);

            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;
            MessageStore = messageStore;

            _currentUser.CurrentUserChange += CurrentUser_CurrentUserChange;
            _currentUser.CurrentAttendanceChange += CurrentUser_CurrentAttendanceChange;
            MessageStore.MessageChanged += MessageStore_MessageChanged;
        }

        private void CurrentUser_CurrentAttendanceChange()
        {
            OnPropertyChanged(nameof(CurrentActivity));
            OnPropertyChanged(nameof(IsCurrentActivitySet));
        }

        private void MessageStore_MessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }

        private void CurrentUser_CurrentUserChange()
        {
            OnPropertyChanged(nameof(CurrentName));
            OnPropertyChanged(nameof(UserLogOn));
            OnPropertyChanged(nameof(UserIsAdmin));
            OnPropertyChanged(nameof(UserIsSupervisor));
            OnPropertyChanged(nameof(IsCurrentActivitySet));
            OnPropertyChanged(nameof(IsButtonPlanVisibile));
        }

        public MessageStore MessageStore { get; private set;  }

        public string CurrentName => _currentUser.User?.LastName + " " + _currentUser.User?.FirstName;

        public string CurrentActivity => _attendanceRecordStore.CurrentAttendanceRecord?.Activity.Name;

        public bool IsCurrentActivitySet => _attendanceRecordStore.CurrentAttendanceRecord?.Activity != null;

        public bool UserLogOn => _currentUser.User != null;

        public bool UserIsAdmin => UserLogOn && _currentUser.User.IsAdmin;

        public bool UserIsSupervisor => UserLogOn && _currentUser.IsUserSuperVisor;    
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersKeysCommand { get; }
        public ICommand NavigateUserMenuCommand { get; }
        public ICommand NavigateProfileCommand { get; }
        public ICommand NavigateGroupsCommand { get; }
        public ICommand NavigateActivitiesCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateHistoryCommnad { get; }
        public ICommand NavigateFixesCommnad { get; }
        public ICommand NavigateRequestsCommand { get; }
        public ICommand NavigateUserPlanCommand { get; }

        public bool IsButtonPlanVisibile => UserLogOn && _currentUser.User.Group.AvailableActivities.Exists(a => a.Property.IsPlan);

        public override void Dispose()
        {
            _currentUser.CurrentUserChange -= CurrentUser_CurrentUserChange;
            base.Dispose();
        }
    }
}
