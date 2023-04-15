using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SetUserToGroupCommand : CommandBase
    {
        private readonly GroupsViewModel _groupsViewModel;
        private readonly UserStore _userStore;
        private readonly GroupStore _groupStore;
        private readonly MessageStore _messageStore;

        public SetUserToGroupCommand(GroupsViewModel groupsViewModel, UserStore userStore, GroupStore groupStore, MessageStore messageStore)
        {
            _groupsViewModel = groupsViewModel;
            _userStore = userStore;
            _groupStore = groupStore;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            Group group = _groupsViewModel.SelectedGroup;
            User user = _groupsViewModel.SelectedUserToSet;
            if (parameter is string value)
            {
                if (value == "addUserToGroup")
                {
                    //_groupStore.AddUserToGroup(group, user);
                    _userStore.SetGroup(user, group);
                    _messageStore.Message = "Uživatel " + user + " přidán do skupiny " + group.Name;
                }
                else if (value == "setSupervisorToGroup")
                {
                    _groupStore.SetSupervisor(user, group);
                    _messageStore.Message = "Uživatel " + user + " nastaven jako vedoucí skupiny " + group.Name;
                }
            }

        }
    }
}
