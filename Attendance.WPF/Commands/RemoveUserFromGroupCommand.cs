using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class RemoveUserFromGroupCommand : CommandBase
    {
        private readonly GroupStore _groupStore;
        private readonly GroupsViewModel _groupViewModel;

        public RemoveUserFromGroupCommand(GroupStore groupStore, GroupsViewModel groupsViewModel)
        {
            _groupStore = groupStore;
            _groupViewModel = groupsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _groupStore.RemoveUserToGroup(_groupViewModel.SelectedGroup, _groupViewModel.SelectedUser);
        }
    }
}
