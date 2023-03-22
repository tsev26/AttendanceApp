using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class ChangeSupervisorOfGroupCommand : CommandBase
    {
        private GroupStore groupStore;

        public ChangeSupervisorOfGroupCommand(GroupStore groupStore)
        {
            this.groupStore = groupStore;
        }

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
