﻿using Attendance.WPF.Services;
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
        private readonly UsersViewModel _usersViewModel;
        private readonly SelectedDataStore _selectedUserStoreUser;
        private readonly INavigationService _navigateModalUpsertKey;
        private readonly MessageStore _messageStore;
        

        public KeyCommand(SelectedDataStore selectedUserStore, UserKeysViewModel userKeysViewModel, MessageStore messageStore, INavigationService navigateModalUpsertKey)
        {
            _selectedUserStoreUser = selectedUserStore;
            _userKeysViewModel = userKeysViewModel;
            _navigateModalUpsertKey = navigateModalUpsertKey;
            _messageStore = messageStore;
        }

        public KeyCommand(SelectedDataStore selectedUserStore, UsersViewModel usersViewModel, MessageStore messageStore, INavigationService navigateUpsertKey)
        {
            _selectedUserStoreUser = selectedUserStore;
            _usersViewModel = usersViewModel;
            _navigateModalUpsertKey = navigateUpsertKey;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string keyOperation)
            {
                
                if (keyOperation == "Remove")
                {
                    if (_usersViewModel != null)
                    {
                        if (_usersViewModel.SelectedKeyIndex != -1)
                        {
                            _messageStore.Message = "Klíč " + _usersViewModel.SelectedKey.KeyValue + " odstraněn";
                            _selectedUserStoreUser.RemoveKey(_selectedUserStoreUser.SelectedUser, _usersViewModel.SelectedKey);
                        }
                        
                    }
                    else if (_userKeysViewModel != null)
                    {
                        if (_userKeysViewModel.SelectedIndex != -1)
                        {
                            _messageStore.Message = "Klíč " + _userKeysViewModel.UsersKeys[_userKeysViewModel.SelectedIndex].KeyValue + " odstraněn";
                            _selectedUserStoreUser.RemoveKey(_selectedUserStoreUser.SelectedUser, _userKeysViewModel.UsersKeys[_userKeysViewModel.SelectedIndex]);
                        }
                    }

                } 
                else if (keyOperation == "Update")
                {
                    if (_usersViewModel != null)
                    {
                        if (_usersViewModel.IsKeySelected)
                        {

                            _navigateModalUpsertKey.Navigate();
                        }
                    }
                    else if (_userKeysViewModel != null)
                    {
                        if (_userKeysViewModel.SelectedIndex != -1)
                        {
                            _navigateModalUpsertKey.Navigate();
                        }
                    }
                }
                else if (keyOperation == "Add")
                {
                    if (_usersViewModel != null)
                    {
                        _usersViewModel.SelectedKeyIndex = -1;
                    }
                    else if (_userKeysViewModel != null)
                    {
                        _userKeysViewModel.SelectedIndex = -1;
                    }
                    
                    _navigateModalUpsertKey.Navigate();
                }
                
            }
        }
    }
}
