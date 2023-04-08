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
        private readonly CurrentUser _currentUser;

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
                                      CurrentUser currentUser)
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
            _currentUser = currentUser;
            _currentUser.CurrentUserChange += CurrentUser_CurrentUserChange;
        }

        private void CurrentUser_CurrentUserChange()
        {
            OnPropertyChanged(nameof(CurrentName));
            OnPropertyChanged(nameof(UserLogOn));
            OnPropertyChanged(nameof(UserIsAdmin));
            OnPropertyChanged(nameof(CurrentActivity));
            OnPropertyChanged(nameof(UserIsSupervisor));
            OnPropertyChanged(nameof(IsCurrentActivitySet));
        }

        public string CurrentName => _currentUser.User?.LastName + " " + _currentUser.User?.FirstName;

        public string CurrentActivity => _currentUser.AttendanceRecordStore.CurrentAttendanceRecord?.Activity.Name;

        public bool IsCurrentActivitySet => _currentUser.AttendanceRecordStore.CurrentAttendanceRecord?.Activity != null;

        public bool UserLogOn => _currentUser.User != null;

        public bool UserIsAdmin => UserLogOn && _currentUser.User.IsAdmin;

        public bool UserIsSupervisor => UserLogOn && _currentUser.User.IsAdmin;

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
        public override void Dispose()
        {
            _currentUser.CurrentUserChange -= CurrentUser_CurrentUserChange;
            base.Dispose();
        }
    }
}
