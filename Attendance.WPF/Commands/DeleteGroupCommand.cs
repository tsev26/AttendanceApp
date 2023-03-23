using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class DeleteGroupCommand : CommandBase
    {
        private readonly GroupStore _groupStore;
        private readonly GroupsViewModel _groupsViewModel;

        public DeleteGroupCommand(GroupStore groupStore, GroupsViewModel groupsViewModel)
        {
            _groupStore = groupStore;
            _groupsViewModel = groupsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _groupStore.RemoveGroup(_groupsViewModel.SelectedGroup);
        }
    }
}
