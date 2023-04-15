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
        private readonly UserStore _userStore;
        private readonly UserProfileViewModel _userProfileViewModel;
        private readonly MessageStore _messageStore;


        public CreateUpdateOnUserCommand(UserStore userStore, UserProfileViewModel userProfileViewModel, MessageStore messageStore)
        {
            _userStore = userStore;
            _userProfileViewModel = userProfileViewModel;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            _userStore.AddUser(_userProfileViewModel.UserUpdate);
            _messageStore.Message = "Vytvořena žádost o opravu profilu";
        }
    }
}
