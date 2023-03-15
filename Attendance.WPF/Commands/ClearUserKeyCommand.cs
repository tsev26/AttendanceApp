using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class ClearUserKeyCommand : CommandBase
    {
        private HomeViewModel _homeViewModel;
        public ClearUserKeyCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }
        public override void Execute(object? parameter)
        {
            _homeViewModel.UserKey = "";
        }
    }
}
