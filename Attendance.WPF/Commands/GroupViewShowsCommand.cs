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
                if (value == "addUser")
                {
                    _groupsViewModel.GroupViewAddUser = true;
                }
                else if (value == "setSupervisor")
                {
                    _groupsViewModel.GroupViewAddUser = false;
                }
                _groupsViewModel.AddUserOrSetSupervisor = true;
            }
            else
            {
                _groupsViewModel.AddUserOrSetSupervisor = false;
            }

        }
    }
}
