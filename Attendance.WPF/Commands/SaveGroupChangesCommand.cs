using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveGroupChangesCommand : CommandBase
    {
        private readonly GroupStore _groupStore;
        private readonly MessageStore _messageStore;
        private readonly GroupsViewModel _groupsViewModel;

        public SaveGroupChangesCommand(GroupStore groupStore, MessageStore messageStore,GroupsViewModel groupsViewModel)
        {
            _groupStore = groupStore;
            _messageStore = messageStore;
            _groupsViewModel = groupsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _groupStore.UpdateGroupObligation(_groupsViewModel.SelectedGroup);
            _messageStore.Message = "Skupina " + _groupsViewModel.SelectedGroup.Name + " upravena";
        }
    }
}
