using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Key = Attendance.Domain.Models.Key;

namespace Attendance.WPF.ViewModels
{
    public class UserKeysViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
        private readonly SelectedDataStore _selectedUserStore;

        public UserKeysViewModel(CurrentUserStore currentUser, SelectedDataStore selectedUserStore, ActivityStore activityStore, MessageStore messageStore, INavigationService navigateUpsertKey, INavigationService navigateExport)
        {
            _currentUser = currentUser;
            _selectedUserStore = selectedUserStore;
            _selectedUserStore.SelectedUser = _currentUser.User;
            NavigateUpsertKey = new KeyCommand(selectedUserStore, this, messageStore, navigateUpsertKey);
            _selectedUserStore.SelectedUserChange += SelectedUserChange_CurrentUserKeysChange;

            MainWorkActivity = activityStore.GlobalSetting.MainWorkActivity;
            SaveFastWorkChangeCommand = new SaveFastWorkChangeCommand(selectedUserStore, messageStore, this);
            _isFastWorkSet = _selectedUserStore.SelectedUser.IsFastWorkSet;

            NavigateExport = new NavigateCommand(navigateExport);

            SelectedUserChange_CurrentUserKeysChange();
        }

        public ICommand NavigateUpsertKey { get; }
        public ICommand SaveFastWorkChangeCommand { get; }
        public ICommand NavigateExport { get; }

        private void SelectedUserChange_CurrentUserKeysChange()
        {
            UsersKeys = _currentUser.User.Keys.Select(a => a.Clone()).ToList();
            OnPropertyChanged(nameof(UsersKeys));

            SelectedIndex = -1;
        }

        public Activity MainWorkActivity { get; private set; }

        private bool _isFastWorkSet;
        public bool IsFastWorkSet
        {
            get
            {
                return _isFastWorkSet;
            }
            set
            {
                _isFastWorkSet = value;
                OnPropertyChanged(nameof(IsFastWorkSet));
                SaveFastWorkChangeCommand.Execute(this);
            }
        }

        public List<Key> UsersKeys { get; set; }

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(IsKeySelected));
                OnPropertyChanged(nameof(SelectedKey));
                _selectedUserStore.SelectedKeyValue = (SelectedIndex != -1) ? UsersKeys[SelectedIndex] : null;
  
            }
        }

        public string SelectedKey => (SelectedIndex != -1) ? UsersKeys[SelectedIndex].KeyValue : "";

        public bool IsKeySelected => SelectedIndex != -1;

        public override void Dispose()
        {
            _selectedUserStore.SelectedUserChange -= SelectedUserChange_CurrentUserKeysChange;
            base.Dispose();
        }
    }
}
