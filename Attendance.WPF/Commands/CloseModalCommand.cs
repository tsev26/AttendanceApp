using Attendance.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class CloseModalCommand : CommandBase
    {
        private INavigationService _closeModalService;

        public CloseModalCommand(INavigationService closeModalService)
        {
            _closeModalService = closeModalService;
        }

        public override void Execute(object? parameter)
        {
            _closeModalService.Navigate();
        }
    }
}
