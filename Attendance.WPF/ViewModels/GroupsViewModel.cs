using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Group = Attendance.Domain.Models.Group;

namespace Attendance.WPF.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        private readonly GroupStore _groupStore;
        private readonly UserStore _userStore;

        public GroupsViewModel(GroupStore groupStore, 
                               UserStore userStore,
                               INavigationService navigateAddGroup)
        {
            _groupStore = groupStore;
            _userStore = userStore;

            NavigateCreateGroupCommand = new NavigateCommand(navigateAddGroup);
            DeleteGroupCommand = new DeleteGroupCommand(_groupStore, this);
            RemoveUserFromGroup = new RemoveUserFromGroupCommand(_groupStore, this);
            SaveGroupChanges = new SaveGroupChangesCommand(_groupStore, this);
            GroupViewShowsCommand = new GroupViewShowsCommand(this);
            SetUserToGroupCommand = new SetUserToGroupCommand(this, _userStore, _groupStore);

            _groupStore.GroupsChange += GroupStore_GroupsChange;

            GroupStore_GroupsChange();
        }


        public IList<User> UsersToSet => (IsGroupSelected) ? ((GroupViewAddUser) 
                                        ? _userStore.Users.Except(SelectedGroup.Users).ToList() 
                                        : _userStore.Users.Where(a => a != SelectedGroup.Supervisor).ToList()) 
                                        : null;

        private int _selectedUserToSetIndex = -1;
        public int SelectedUserToSetIndex
        {
            get
            {
                return _selectedUserToSetIndex;
            }
            set
            {
                _selectedUserToSetIndex = value;
                OnPropertyChanged(nameof(SelectedUserToSetIndex));
            }
        }

        public User? SelectedUserToSet => (SelectedUserToSetIndex != -1) ? UsersToSet[SelectedUserToSetIndex] : null;

        private void GroupStore_GroupsChange()
        {
            Groups = _groupStore.Groups.Select(a => a.Clone(a)).ToList();
            OnPropertyChanged(nameof(Groups));
        }

        /*
        private void GroupSelectionChange()
        {
            Groups.Clear();
            foreach (var group in _groupStore.Groups.ToList())
            {
                Groups.Add(group);
            }
        }
        */

        public ICommand SetUserToGroupCommand { get; }
        public ICommand NavigateCreateGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand RemoveUserFromGroup { get; }
        public ICommand SaveGroupChanges { get; }
        public ICommand GroupViewShowsCommand { get; }


        public List<Group> Groups { get; set; }

        private int _selectedGroupIndex = -1;
        public int SelectedGroupIndex
        {
            get
            {
                return _selectedGroupIndex;
            }
            set
            {
                _selectedGroupIndex = value;
                OnPropertyChanged(nameof(SelectedGroupIndex));
                OnPropertyChanged(nameof(IsGroupSelected));
                OnPropertyChanged(nameof(SelectedGroup));
                OnPropertyChanged(nameof(AddUserButtonVisibility));
                OnPropertyChanged(nameof(SettingButtonVisibility));
                OnPropertyChanged(nameof(UsersToSet));

            }
        }

        public bool IsGroupSelected => SelectedGroupIndex != -1;

        public Group SelectedGroup => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex] : null;

        private int _selectedUserIndex = -1;
        public int SelectedUserIndex
        {
            get
            {
                return _selectedUserIndex;
            }
            set
            {
                _selectedUserIndex = value;
                OnPropertyChanged(nameof(SelectedUserIndex));
                OnPropertyChanged(nameof(IsUserSelected));
                OnPropertyChanged(nameof(SelectedUser));

                OnPropertyChanged(nameof(HasRegularWorkingTime));
                OnPropertyChanged(nameof(MinHoursWorked));
                OnPropertyChanged(nameof(LatestArival));
                OnPropertyChanged(nameof(EarliestDeparture));
                OnPropertyChanged(nameof(WorksMonday));
                OnPropertyChanged(nameof(WorksTuesday));
                OnPropertyChanged(nameof(WorksWednesday));
                OnPropertyChanged(nameof(WorksThursday));
                OnPropertyChanged(nameof(WorksFriday));
                OnPropertyChanged(nameof(WorksSaturday));
                OnPropertyChanged(nameof(WorksSunday));
            }
        }

        public User SelectedUser => (SelectedUserIndex != 1) ? SelectedGroup.Users[SelectedUserIndex] : null;

        private bool _addUserOrSetSupervisor;
        public bool AddUserOrSetSupervisor
        {
            get
            {
                return _addUserOrSetSupervisor;
            }
            set
            {
                _addUserOrSetSupervisor = value;
                OnPropertyChanged(nameof(AddUserOrSetSupervisor));
                OnPropertyChanged(nameof(GroupSetting));
                OnPropertyChanged(nameof(AddUserButtonVisibility));
                OnPropertyChanged(nameof(SettingButtonVisibility));
            }
        }

        private bool _groupViewAddUser;
        public bool GroupViewAddUser
        {
            get
            {
                return _groupViewAddUser;
            }
            set
            {
                _groupViewAddUser = value;
                OnPropertyChanged(nameof(GroupViewAddUser));
                OnPropertyChanged(nameof(GroupViewSetUser));
                OnPropertyChanged(nameof(AddUserButtonVisibility));
                OnPropertyChanged(nameof(GroupViewAddUserOrSetSupervisorText));
                OnPropertyChanged(nameof(UsersToSet));
            }
        }

        public bool GroupViewSetUser => !GroupViewAddUser;

        public string GroupViewAddUserOrSetSupervisorText => GroupViewAddUser ? "Přidat uživatele" : "Nastavit vedoucího";


        public bool GroupSetting => !AddUserOrSetSupervisor && IsGroupSelected;

        public bool AddUserButtonVisibility => (GroupSetting || !GroupViewAddUser) && IsGroupSelected;

        public bool SettingButtonVisibility => IsGroupSelected;

        public bool IsUserSelected => SelectedUserIndex != -1;

        public bool HasRegularWorkingTime => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.HasRegularWorkingTime : false;

        public double MinHoursWorked => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.MinHoursWorked : 0;

        public TimeOnly LatestArival => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.LatestArival : new TimeOnly();

        public TimeOnly EarliestDeparture => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.EarliestDeparture : new TimeOnly();

        public bool WorksMonday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksMonday : false;
        public bool WorksTuesday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksTuesday : false;
        public bool WorksWednesday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksWednesday : false;
        public bool WorksThursday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksThursday : false;
        public bool WorksFriday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksFriday : false;
        public bool WorksSaturday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksSaturday : false;
        public bool WorksSunday => (SelectedGroupIndex != -1) ? Groups[SelectedGroupIndex].Obligation.WorksSunday : false;


        public override void Dispose()
        {
            _groupStore.GroupsChange -= GroupStore_GroupsChange;
            base.Dispose();
        }
    }
}
