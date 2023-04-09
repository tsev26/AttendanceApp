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

        public SetActivityToGroupCommand(GroupsViewModel groupsViewModel, GroupStore groupStore, ActivityStore activityStore)
        {
            _groupsViewModel = groupsViewModel;
            _groupStore = groupStore;
            _activityStore = activityStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string value)
            {
                if (value == "addActivityToGroup")
                {
                    
                }
                else if (value == "removeActivityToGroup")
                {

                }
            }
        }
    }
}
