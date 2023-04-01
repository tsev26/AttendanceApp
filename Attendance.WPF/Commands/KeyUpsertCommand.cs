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
        private readonly SelectedUserStore _selectedUserStore;
        private readonly INavigationService _closeAndReloadModalService;

        public KeyUpsertCommand(SelectedUserStore selectedUserStore, INavigationService closeAndReloadModalService)
        {
            _selectedUserStore = selectedUserStore;
            _closeAndReloadModalService = closeAndReloadModalService;
        }

        public override void Execute(object? parameter)
        {
            _selectedUserStore.UpsertKey(_selectedUserStore.SelectedUser, _selectedUserStore.SelectedKeyValue);
            _closeAndReloadModalService.Navigate();
        }
    }
}
