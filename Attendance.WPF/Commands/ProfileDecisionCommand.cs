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
        private readonly MessageStore _messageStore;
        private readonly UsersRequestsViewModel _usersRequestsViewModel;

        public ProfileDecisionCommand(UserStore userStore, MessageStore messageStore, UsersRequestsViewModel usersRequestsViewModel)
        {
            _userStore = userStore;
            _messageStore = messageStore;
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
                    _messageStore.Message = "Žádost o změnu profilu schválena";
                } 
                else if (value == "Reject")
                {
                    _userStore.DeleteUser(_usersRequestsViewModel.SelectedPendingProfileUpdate);
                    _messageStore.Message = "Žádost o změnu profilu zamítnuta";
                }
            }
        }
    }
}
