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
        private readonly CurrentUserStore _currentUser;
        private readonly UserKeysViewModel _userKeysViewModel;
        private readonly SelectedDataStore _selectedUserStore;



        public SaveFastWorkChangeCommand(SelectedDataStore selectedUserStore, UserKeysViewModel userKeysViewModel)
        {
            _selectedUserStore = selectedUserStore;
            _userKeysViewModel = userKeysViewModel;
        }

        public override void Execute(object? parameter)
        {
            _selectedUserStore.SetFastWork(_userKeysViewModel.IsFastWorkSet);
        }
    }
}
