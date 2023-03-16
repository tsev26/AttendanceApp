using Attendance.Domain.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class UserSetActivityCommand : AsyncCommandBase
    {
        private readonly INavigationService _navigationService;
        private readonly CurrentUser _currentUser;

        public UserSetActivityCommand(INavigationService navigationService, CurrentUser currentUser)
        {
            _navigationService = navigationService;
            _currentUser = currentUser;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is Activity activity)
            {
                _currentUser.SetActivity(activity);
                _navigationService.Navigate();
            }
        }
    }
}
