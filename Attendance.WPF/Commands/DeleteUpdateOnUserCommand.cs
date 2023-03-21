using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class DeleteUpdateOnUserCommand : CommandBase
    {
        private CurrentUser _currentUser;
        private UserStore _userStore;

        public DeleteUpdateOnUserCommand(CurrentUser currentUser, UserStore userStore)
        {
            _currentUser = currentUser;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            _userStore.DeleteUser(_currentUser.UserUpdates);
        }
    }
}
