using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UserUpsertViewModel : ViewModelBase
    {
        private UserStore _userStore;
        private GroupStore _groupStore;

        public UserUpsertViewModel(UserStore userStore, GroupStore groupStore, CloseModalNavigationService closeModalNavigationService)
        {
            _userStore = userStore;
            _groupStore = groupStore;

            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            CreateUserCommand = new CreateUserCommand(userStore, this, closeModalNavigationService);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand CreateUserCommand { get; }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        public List<Group> Groups => _groupStore.Groups;

        private string _keyValue;
        public string KeyValue
        {
            get
            {
                return _keyValue;
            }
            set
            {
                _keyValue = value;
                OnPropertyChanged(nameof(KeyValue));
            }
        }

    }
}
