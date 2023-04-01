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
        private GroupsViewModel _groupsViewModel;
        private UserStore _userStore;
        private GroupStore _groupStore;

        public SetUserToGroupCommand(GroupsViewModel groupsViewModel, UserStore userStore, GroupStore groupStore)
        {
            _groupsViewModel = groupsViewModel;
            _userStore = userStore;
            _groupStore = groupStore;
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
                }
                else if (value == "setSupervisorToGroup")
                {
                    _groupStore.SetSupervisor(user, group);
                }
            }

        }
    }
}
