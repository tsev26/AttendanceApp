using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class KeyUpsertCommand : CommandBase
    {
        private CurrentUser _currentUser;
        private INavigationService _closeAndReloadModalService;

        public KeyUpsertCommand(CurrentUser currentUser, INavigationService closeAndReloadModalService)
        {
            _currentUser = currentUser;
            _closeAndReloadModalService = closeAndReloadModalService;
        }

        public override void Execute(object? parameter)
        {
            _currentUser.UpsertKey(_currentUser.SelectedKeyValue);
            _closeAndReloadModalService.Navigate();
        }
    }
}
