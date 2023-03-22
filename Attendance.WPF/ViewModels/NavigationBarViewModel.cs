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
                                      CurrentUser currentUser)
        {
            NavigateHomeCommand = new NavigateCommand(navigateHomeService);
            NavigateUsersKeysCommand = new NavigateCommand(navigateUsersKeysService);
            NavigateUserMenuCommand = new NavigateCommand(navigateUserMenuService);
            NavigateProfileCommand = new NavigateCommand(navigateUserProfileService);
            NavigateGroupsCommand = new NavigateCommand(navigateGroupsService);
            _currentUser = currentUser;
            _currentUser.CurrentUserChange += CurrentUser_CurrentUserChange;
        }

        private void CurrentUser_CurrentUserChange()
        {
            OnPropertyChanged(nameof(CurrentName));
            OnPropertyChanged(nameof(UserLogOn));
            OnPropertyChanged(nameof(UserIsAdmin));
        }

        public string CurrentName => _currentUser.User?.LastName + " " + _currentUser.User?.FirstName;


        public bool UserLogOn => _currentUser.User != null;

        public bool UserIsAdmin => UserLogOn && _currentUser.User.IsAdmin;

        public bool UserIsSupervisor => UserLogOn && _currentUser.User.IsAdmin;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersKeysCommand { get; }
        public ICommand NavigateUserMenuCommand { get; }
        public ICommand NavigateProfileCommand { get; }
        public ICommand NavigateGroupsCommand { get; }

        public override void Dispose()
        {
            _currentUser.CurrentUserChange -= CurrentUser_CurrentUserChange;
            base.Dispose();
        }
    }
}
