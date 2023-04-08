using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class ProfileDecisionCommand : CommandBase
    {
        private readonly UserStore _userStore;
        private readonly UsersRequestsViewModel _usersRequestsViewModel;

        public ProfileDecisionCommand(UserStore userStore, UsersRequestsViewModel usersRequestsViewModel)
        {
            _userStore = userStore;
            _usersRequestsViewModel = usersRequestsViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                if (value == "Approve")
                {
                    _userStore.UpdateUser(_usersRequestsViewModel.SelectedPendingProfileUpdate);
                    _userStore.DeleteUser(_usersRequestsViewModel.SelectedPendingProfileUpdate);
                } 
                else if (value == "Reject")
                {
                    _userStore.DeleteUser(_usersRequestsViewModel.SelectedPendingProfileUpdate);
                }
            }
        }
    }
}
