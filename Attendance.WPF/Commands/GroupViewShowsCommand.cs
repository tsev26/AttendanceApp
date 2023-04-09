using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class GroupViewShowsCommand : CommandBase
    {
        private GroupsViewModel _groupsViewModel;

        public GroupViewShowsCommand(GroupsViewModel groupsViewModel)
        {
            _groupsViewModel = groupsViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                _groupsViewModel.AddUserOrSetSupervisor = false;
                _groupsViewModel.SetActivities = false;
                _groupsViewModel.GroupSetting = false;
                switch (value)
                {
                    case "addUser":
                        _groupsViewModel.GroupViewAddUser = true;
                        _groupsViewModel.AddUserOrSetSupervisor = true;
                        break;
                    case "setSupervisor":
                        _groupsViewModel.GroupViewAddUser = false;
                        _groupsViewModel.AddUserOrSetSupervisor = true;
                        break;
                    case "activities":
                        _groupsViewModel.SetActivities = true;
                        break;
                    case "obligation":
                        _groupsViewModel.GroupSetting = true;
                        break;
                }
            }
        }
    }
}
