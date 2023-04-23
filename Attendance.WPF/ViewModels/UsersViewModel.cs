using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Key = Attendance.Domain.Models.Key;

namespace Attendance.WPF.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly GroupStore _groupStore;
        private readonly SelectedDataStore _selectedUserStore;
        public UsersViewModel(UserStore userStore, CurrentUserStore currentUser, GroupStore groupStore, SelectedDataStore selectedUserStore, MessageStore messageStore, INavigationService navigateUserUpsert, INavigationService navigateUpsertKey)
        {
            _userStore = userStore;
            CurrentUser = currentUser;
            _groupStore = groupStore;
            _selectedUserStore = selectedUserStore;

            SaveUserCommand = new SaveUserCommand(userStore, this, messageStore);
            CreateUserNavigateCommand = new NavigateCommand(navigateUserUpsert);
            UsersViewShowsCommand = new UsersViewShowsCommand(this);
            NavigateUpsertKey = new KeyCommand(selectedUserStore, this, messageStore, navigateUpsertKey);

            _userStore.UsersChange += UserStore_UsersChange;
            _selectedUserStore.SelectedUserChange += SelectedUserStore_SelectedUserChange;

            ShowUsersProfile = true;
            ShowUsersKeys = false;
            ShowUsersAttendace = false;

            LoadUsers(CurrentUser.User);
            _groupStore.LoadGroups();
        }
        private void SelectedUserStore_SelectedUserChange()
        {
            if (!IsUserSelected) return;
            UsersKeys = SelectedUser.Keys.ToList(); //.Select(a => a.Clone())
            OnPropertyChanged(nameof(UsersKeys));

            SelectedKeyIndex = -1;
        }
      
        private async Task LoadUsers(User user)
        {
            await _userStore.LoadUsers(user);
            Users = _userStore.Users.ToList();
            OnPropertyChanged(nameof(Users));
        }

        private void UserStore_UsersChange()
        {
            Users = _userStore.Users.ToList();
            OnPropertyChanged(nameof(Users));

            SelectedUserIndex = -1;
        }

        public ICommand SaveUserCommand { get; }
        public ICommand CreateUserNavigateCommand { get; }
        public ICommand UsersViewShowsCommand { get; }
        public ICommand NavigateUpsertKey { get; }

        public CurrentUserStore CurrentUser { get; }
        public List<Group> Groups => _groupStore.Groups;

        public List<User> Users { get; set; }

        private int _selectedUserIndex = -1;
        public int SelectedUserIndex
        {
            get { return _selectedUserIndex; }
            set
            {
                _selectedUserIndex = value;
                OnPropertyChanged(nameof(SelectedUserIndex));
                OnPropertyChanged(nameof(IsUserSelected));
                OnPropertyChanged(nameof(SelectedUser));
                _selectedUserStore.SelectedUser = SelectedUser;
                SelectedUserObligation = IsUserSelected ? new Obligation(SelectedUser.UserObligation) : null;
                SelectedUserStore_SelectedUserChange();
            }
        }

        public bool IsUserSelected => SelectedUserIndex != -1;
        public User SelectedUser => IsUserSelected ? Users[SelectedUserIndex] : null;

        private Obligation _selectedUserObligation;
        public Obligation SelectedUserObligation
        {
            get
            {
                return _selectedUserObligation;
            }
            set
            {
                _selectedUserObligation = value;
                OnPropertyChanged(nameof(SelectedUserObligation));
            }
        }

        private bool _showUsersProfile = false;
        public bool ShowUsersProfile
        {
            get
            {
                return _showUsersProfile;
            }
            set
            {
                _showUsersProfile = value;
                OnPropertyChanged(nameof(ShowUsersProfile));
            }
        }

        private bool _showUsersKeys;
        public bool ShowUsersKeys
        {
            get
            {
                return _showUsersKeys;
            }
            set
            {
                _showUsersKeys = value;
                OnPropertyChanged(nameof(ShowUsersKeys));
                SelectedUserStore_SelectedUserChange();
            }
        }

        private bool _showUsersAttendace;
        public bool ShowUsersAttendace
        {
            get
            {
                return _showUsersAttendace;
            }
            set
            {
                _showUsersAttendace = value;
                OnPropertyChanged(nameof(ShowUsersAttendace));
            }
        }

        public List<Key> UsersKeys { get; set; }

        private int _selectedKeyIndex = -1;
        public int SelectedKeyIndex
        {
            get
            {
                return _selectedKeyIndex;
            }
            set
            {
                _selectedKeyIndex = value;
                OnPropertyChanged(nameof(SelectedKeyIndex));
                OnPropertyChanged(nameof(IsKeySelected));
                OnPropertyChanged(nameof(SelectedKey));
                _selectedUserStore.SelectedKeyValue = IsKeySelected ? UsersKeys[SelectedKeyIndex] : null;
            }
        }

        public bool IsKeySelected => SelectedKeyIndex != -1;
        public Key SelectedKey => IsKeySelected ? UsersKeys[SelectedKeyIndex] : null;

        public override void Dispose()
        {
            _userStore.UsersChange -= UserStore_UsersChange;
            _selectedUserStore.SelectedUserChange -= SelectedUserStore_SelectedUserChange;
            _selectedUserStore.SelectedKeyValue = null;
            base.Dispose();
        }
    }
}
