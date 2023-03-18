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

        public NavigationBarViewModel(INavigationService navigateHomeService, CurrentUser currentUser)
        {
            NavigateHomeCommand = new NavigateCommand(navigateHomeService);
            _currentUser = currentUser;
        }

        public string CurrentName => "test";//_currentUser.User.LastName + " " + _currentUser.User.FirstName;

        public ICommand NavigateHomeCommand { get; set; }
    }
}
