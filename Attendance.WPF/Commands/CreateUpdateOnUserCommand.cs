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
        private UserStore _userStore;
        private UserProfileViewModel _userProfileViewModel;


        public CreateUpdateOnUserCommand(UserStore userStore, UserProfileViewModel userProfileViewModel)
        {
            _userStore = userStore;
            _userProfileViewModel = userProfileViewModel;
        }

        public override void Execute(object? parameter)
        {
            _userStore.AddUser(_userProfileViewModel.UserUpdate);
        }
    }
}
