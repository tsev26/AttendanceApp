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
        private readonly ActivityStore _activityStore;

        public SetActivityToGroupCommand(GroupsViewModel groupsViewModel, GroupStore groupStore)
        {
            _groupsViewModel = groupsViewModel;
            _groupStore = groupStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                if (value == "addActivityToGroup")
                {
                    _groupStore.AddActivityToGroup(_groupsViewModel.SelectedGroup, _groupsViewModel.SelectedActivityNotAssignedGroup);
                }
                else if (value == "removeActivityToGroup")
                { 
                    _groupStore.RemoveActivityFromGroup(_groupsViewModel.SelectedGroup, _groupsViewModel.SelectedActivityGroup);
                }
            }
        }
    }
}
