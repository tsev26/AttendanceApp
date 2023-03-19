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

namespace Attendance.WPF.ViewModels
{
    public class UserKeysViewModel : ViewModelBase
    {
        private CurrentUser _currentUser;

        public UserKeysViewModel(CurrentUser currentUser, INavigationService navigateUpsertKey)
        {
            _currentUser = currentUser;
            NavigateUpsertKey = new KeyCommand(currentUser,this,navigateUpsertKey);
            _currentUser.CurrentUserKeysChange += CurrentUser_CurrentUserKeysChange;
            UsersKeys = new ObservableCollection<Attendance.Domain.Models.Key>();
            LoadUsersKeys();
        }

        public ICommand NavigateUpsertKey { get; }

        private void CurrentUser_CurrentUserKeysChange()
        {

            LoadUsersKeys();
            SelectedIndex = -1;
        }

        private void LoadUsersKeys()
        {
            UsersKeys.Clear();
            UsersKeys = new ObservableCollection<Attendance.Domain.Models.Key>(_currentUser.User.Keys);
            OnPropertyChanged(nameof(UsersKeys));
        }

        public ObservableCollection<Attendance.Domain.Models.Key> UsersKeys { get; set; }


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
                _currentUser.SelectedKeyValue = (SelectedIndex != -1) ? UsersKeys[SelectedIndex] : null;
  
            }
        }

        public string SelectedKey => (SelectedIndex != -1) ? UsersKeys[SelectedIndex].KeyValue : "";

        public bool IsKeySelected => SelectedIndex != -1;

        public override void Dispose()
        {
            _currentUser.CurrentUserKeysChange -= CurrentUser_CurrentUserKeysChange;
            base.Dispose();
        }
    }
}
