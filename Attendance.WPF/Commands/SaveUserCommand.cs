using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveUserCommand : CommandBase
    {
        private UserStore _userStore;
        private UsersViewModel _usersViewModel;

        public SaveUserCommand(UserStore userStore, UsersViewModel usersViewModel)
        {
            _userStore = userStore;
            _usersViewModel = usersViewModel;
        }

        public override void Execute(object? parameter)
        {
            _userStore.UpdateUser(_usersViewModel.SelectedUser);
        }
    }
}
