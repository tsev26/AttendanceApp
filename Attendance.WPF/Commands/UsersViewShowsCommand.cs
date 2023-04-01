using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class UsersViewShowsCommand : CommandBase
    {
        private UsersViewModel _usersViewModel;

        public UsersViewShowsCommand(UsersViewModel usersViewModel)
        {
            _usersViewModel = usersViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                if (value == "keys")
                {
                    _usersViewModel.ShowUsersProfile = false;
                    _usersViewModel.ShowUsersAttendace = false;
                    _usersViewModel.ShowUsersKeys = true;
                }
                else if (value == "profil")
                {
                    _usersViewModel.ShowUsersProfile = true;
                    _usersViewModel.ShowUsersAttendace = false;
                    _usersViewModel.ShowUsersKeys = false;
                }
                else if (value == "attendance")
                {
                    _usersViewModel.ShowUsersProfile = false;
                    _usersViewModel.ShowUsersAttendace = true;
                    _usersViewModel.ShowUsersKeys = false;
                }
            }
        }
    }
}
