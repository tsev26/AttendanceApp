﻿using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class RemoveUserFromGroupCommand : CommandBase
    {
        private GroupStore _groupStore;

        public RemoveUserFromGroupCommand(GroupStore groupStore)
        {
            _groupStore = groupStore;
        }

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
