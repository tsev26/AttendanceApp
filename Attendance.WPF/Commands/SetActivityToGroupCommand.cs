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
    public class SetActivityToGroupCommand : CommandBase
    {
        private readonly GroupsViewModel _groupsViewModel;
        private readonly GroupStore _groupStore;
        private readonly MessageStore _messageStore;

        public SetActivityToGroupCommand(GroupsViewModel groupsViewModel, GroupStore groupStore, MessageStore messageStore)
        {
            _groupsViewModel = groupsViewModel;
            _groupStore = groupStore;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                if (value == "addActivityToGroup")
                {
                    Activity activityToAssign = _groupsViewModel.SelectedActivityNotAssignedGroup;
                    _groupStore.AddActivityToGroup(_groupsViewModel.SelectedGroup, activityToAssign);
                    _messageStore.Message = "Aktivita " + activityToAssign + " přidána do skupiny " + _groupsViewModel.SelectedGroup.Name;
                }
                else if (value == "removeActivityToGroup")
                {
                    Activity activityToRemove = _groupsViewModel.SelectedActivityGroup;
                    _groupStore.RemoveActivityFromGroup(_groupsViewModel.SelectedGroup, activityToRemove);
                    _messageStore.Message = "Aktivita " + activityToRemove + " odebrána ze skupiny " + _groupsViewModel.SelectedGroup.Name;
                }
            }
        }
    }
}
