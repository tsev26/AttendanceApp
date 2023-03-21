using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class CreateUpdateOnUserCommand : CommandBase
    {
        private CurrentUser _currentUser;
        private UserStore _userStore;

        public CreateUpdateOnUserCommand(CurrentUser currentUser, UserStore userStore)
        {
            _currentUser = currentUser;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            _userStore.AddUser(_currentUser.UserUpdates);
        }
    }
}
