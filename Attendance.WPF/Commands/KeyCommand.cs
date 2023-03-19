using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class KeyCommand : CommandBase
    {
        private readonly UserKeysViewModel _userKeysViewModel;
        private readonly CurrentUser _currentUser;
        private INavigationService _navigateModalUpsertKey;

        public KeyCommand(CurrentUser currentUser, UserKeysViewModel userKeysViewModel, INavigationService navigateModalUpsertKey)
        {
            _currentUser = currentUser;
            _userKeysViewModel = userKeysViewModel;
            _navigateModalUpsertKey = navigateModalUpsertKey;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string keyOperation)
            {
                if (keyOperation == "Remove")
                {
                    if (_userKeysViewModel.SelectedIndex != -1)
                    {
                        _currentUser.RemoveKey(_userKeysViewModel.UsersKeys[_userKeysViewModel.SelectedIndex]);
                    }
                } 
                else if (keyOperation == "Update")
                {
                    if (_userKeysViewModel.SelectedIndex != -1)
                    {
                        _navigateModalUpsertKey.Navigate();
                    }
                }
                else if (keyOperation == "Add")
                {
                    _userKeysViewModel.SelectedIndex = -1;
                    _navigateModalUpsertKey.Navigate();
                }
            }
        }
    }
}
