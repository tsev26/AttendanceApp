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
    public class SaveUserCommand : AsyncCommandBase
    {
        private readonly UserStore _userStore;
        private readonly UsersViewModel _usersViewModel;
        private readonly MessageStore _messageStore;

        public SaveUserCommand(UserStore userStore, UsersViewModel usersViewModel, MessageStore messageStore)
        {
            _userStore = userStore;
            _usersViewModel = usersViewModel;
            _messageStore = messageStore;
        }


        public override async Task ExecuteAsync(object? parameter)
        {
            User user = _usersViewModel.SelectedUser.Clone();
            user.Obligation = _usersViewModel.SelectedUserObligation;
            if (user.Obligation == user.Group.Obligation)
            {
                user.Obligation = null;
            }
            await _userStore.UpdateUser(user);
            _messageStore.Message = "Profil uživatele " + user + " upraven";
        }
    }
}
