using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class KeyUpsertCommand : AsyncCommandBase
    {
        private readonly SelectedDataStore _selectedUserStore;
        private readonly INavigationService _closeAndReloadModalService;
        private readonly MessageStore _messageStore;

        public KeyUpsertCommand(SelectedDataStore selectedUserStore, MessageStore messageStore, INavigationService closeAndReloadModalService)
        {
            _selectedUserStore = selectedUserStore;
            _messageStore = messageStore;
            _closeAndReloadModalService = closeAndReloadModalService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            bool change = await _selectedUserStore.UpsertKey(_selectedUserStore.SelectedUser, _selectedUserStore.SelectedKeyValue);

            if (!change)
            {
                _messageStore.Message = "Klíč " + _selectedUserStore.SelectedKeyValue + " již existuje u jiného uživatele";
            }
            else
            {
                _messageStore.Message = "Klíč " + _selectedUserStore.SelectedKeyValue + " zapsán";
            }
            _closeAndReloadModalService.Navigate();
        }
    }
}
