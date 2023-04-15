using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveFastWorkChangeCommand : CommandBase
    {
        private readonly MessageStore _messageStore;
        private readonly UserKeysViewModel _userKeysViewModel;
        private readonly SelectedDataStore _selectedUserStore;



        public SaveFastWorkChangeCommand(SelectedDataStore selectedUserStore, MessageStore messageStore, UserKeysViewModel userKeysViewModel)
        {
            _selectedUserStore = selectedUserStore;
            _userKeysViewModel = userKeysViewModel;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            _selectedUserStore.SetFastWork(_userKeysViewModel.IsFastWorkSet);
            if (_userKeysViewModel.IsFastWorkSet)
            {
                _messageStore.Message = "Nastaven rychlý zápis hlavní pracovní aktivity";
            }
            else
            {
                _messageStore.Message = "Zrušen rychlý zápis hlavní pracovní aktivity";
            }
            
        }
    }
}
