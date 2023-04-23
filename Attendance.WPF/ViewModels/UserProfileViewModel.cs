﻿using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        public UserProfileViewModel(CurrentUserStore currentUser, UserStore userStore, MessageStore messageStore)
        {
            CurrentUser = currentUser;
            _userStore = userStore;

            UserUpdates = new ObservableCollection<User>();
            UpdatesOnUserCommand = new CreateUpdateOnUserCommand(userStore, this, messageStore);
            DeleteUpdateOnUserCommand = new DeleteUpdateOnUserCommand(userStore, this, messageStore);

            SetCurrentUserUpdates();

            UserStore_UsersChange();
            _userStore.UsersChange += UserStore_UsersChange;

        }

        private void UserStore_UsersChange()
        {
            LoadFixProfile();
        }

        public async Task LoadFixProfile()
        {
            UserUpdates.Clear();
            List<User> users = await _userStore.LoadFixProfile(CurrentUser.User);
            foreach (var user in users)
            {
                UserUpdates.Add(user);
            }
        }

        private void SetCurrentUserUpdates()
        {
            if (IsSelected)
            {
                UserUpdate = UserUpdates[SelectedIndex];
            }
            else
            {
                UserUpdate = CurrentUser.User.Clone();
            }
            ObligationFrom = UserUpdate.HasObligationString;
            UserUpdate.Keys = CurrentUser.User.Keys;
            UserUpdate.Group = CurrentUser.User.Group;
            if (UserUpdate.Obligation == null)
            {
                ObligationFromUser = false;
                UserUpdate.Obligation = UserUpdate.Group.Obligation.Clone();
            }
            else
            {
                ObligationFromUser = true;
                UserUpdate.Obligation = UserUpdate.Obligation.Clone();
            }

            UserUpdate.ToApprove = true;
            
        }

        public bool IsSelected => SelectedIndex != -1;

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                
                OnPropertyChanged(nameof(IsSelected));
                SetCurrentUserUpdates();
                OnPropertyChanged(nameof(UserUpdate));
                OnPropertyChanged(nameof(ObligationFromUser));
                OnPropertyChanged(nameof(ObligationFrom));
            }
        }

        public User UserUpdate { get; set; }

        public CurrentUserStore CurrentUser { get; set; }

        public ObservableCollection<User> UserUpdates { get; set; }

        public ICommand UpdatesOnUserCommand { get; }
        public ICommand DeleteUpdateOnUserCommand { get; }

        public bool ObligationFromUser { get; set; }

        public string ObligationFrom { get; private set; } 

        public override void Dispose()
        { 
            _userStore.UsersChange -= UserStore_UsersChange;
            base.Dispose();
        }
    }
}
