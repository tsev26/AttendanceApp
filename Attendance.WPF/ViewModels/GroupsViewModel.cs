using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Models;
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
        private readonly ActivityStore _activityStore;

        public GroupsViewModel(GroupStore groupStore,
                               UserStore userStore,
                               ActivityStore activityStore,
                               INavigationService navigateAddGroup)
        {
            _groupStore = groupStore;
            _userStore = userStore;
            _activityStore = activityStore;

            NavigateCreateGroupCommand = new NavigateCommand(navigateAddGroup);
            DeleteGroupCommand = new DeleteGroupCommand(_groupStore, this);
            SaveGroupChanges = new SaveGroupChangesCommand(_groupStore, this);
            GroupViewShowsCommand = new GroupViewShowsCommand(this);
            SetUserToGroupCommand = new SetUserToGroupCommand(this, _userStore, _groupStore);
            SetActivityToGroupCommand = new SetActivityToGroupCommand(this, groupStore);

            Groups = new List<Group>(_groupStore.Groups);
            ActivitiesGroup = new List<Activity>();
            ActivitiesNotAssignedGroup = new List<Activity>();

            _groupStore.GroupsChange += GroupStore_GroupsChange;
            _userStore.UsersChange += UserStore_UsersChange;
            _groupStore.GroupsActivitiesChange += GroupStore_GroupsActivitiesChange;
            GroupStore_GroupsChange();
        }



        public List<User> UsersToSet => (IsGroupSelected) ? ((GroupViewAddUser)
                                        ? _userStore.Users.Where(a => a.Group != SelectedGroup && !a.ToApprove).ToList()
                                        : _userStore.Users.Where(a => a != SelectedGroup.Supervisor && !a.ToApprove).ToList())
                                        : null;

        public List<User> UsersInGroup => (IsGroupSelected) ? _userStore.Users.Where(a => a.Group == SelectedGroup && !a.ToApprove).ToList() : null;

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
            Groups = _groupStore.Groups.ToList();
            OnPropertyChanged(nameof(Groups));
        }

        private void UserStore_UsersChange()
        {
            OnPropertyChanged(nameof(UsersInGroup));
            OnPropertyChanged(nameof(UsersToSet));
        }

        private void GroupStore_GroupsActivitiesChange()
        {
            if (IsGroupSelected && SetActivities)
            {
                ActivitiesGroup = SelectedGroup.Obligation.AvailableActivities.ToList();
                OnPropertyChanged(nameof(ActivitiesGroup));

                ActivitiesNotAssignedGroup = _activityStore.Activities.Where(a => !ActivitiesGroup.Contains(a)).ToList();
                OnPropertyChanged(nameof(ActivitiesNotAssignedGroup));
            } 

        }

        public ICommand SetUserToGroupCommand { get; }
        public ICommand NavigateCreateGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand SaveGroupChanges { get; }
        public ICommand GroupViewShowsCommand { get; }
        public ICommand SetActivityToGroupCommand { get; }

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
                OnPropertyChanged(nameof(UsersInGroup));
                OnPropertyChanged(nameof(AddUserButtonVisibility));
                OnPropertyChanged(nameof(SettingButtonVisibility));
                OnPropertyChanged(nameof(UsersToSet));
                GroupStore_GroupsActivitiesChange();
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

        public User SelectedUser => (SelectedUserIndex != 1) ? UsersInGroup[SelectedUserIndex] : null;

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

        private bool _setSupervisor;
        public bool SetSupervisor
        {
            get
            {
                return _setSupervisor;
            }
            set
            {
                _setSupervisor = value;
                OnPropertyChanged(nameof(SetSupervisor));
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

        private bool _groupSetting;
        public bool GroupSetting
        {
            get
            {
                return _groupSetting;
            }
            set
            {
                _groupSetting = value;
                OnPropertyChanged(nameof(GroupSetting));
            }
        }

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


        private bool _setActivities;
        public bool SetActivities
        {
            get
            {
                return _setActivities;
            }
            set
            {
                _setActivities = value;
                OnPropertyChanged(nameof(SetActivities));
                GroupStore_GroupsActivitiesChange();
            }
        }

        public List<Activity> ActivitiesGroup { get; set; } 
        // => IsGroupSelected && SetActivities ? SelectedGroup.Obligation.AvailableActivities : null;

        private int _selectedActivityGroupIndex = -1;
        public int SelectedActivityGroupIndex
        {
            get
            {
                return _selectedActivityGroupIndex;
            }
            set
            {
                _selectedActivityGroupIndex = value;
                OnPropertyChanged(nameof(SelectedActivityGroupIndex));
                OnPropertyChanged(nameof(IsSelectedActivityGroupIndex));
                OnPropertyChanged(nameof(SelectedActivityGroup));
            }
        }
        public bool IsSelectedActivityGroupIndex => SelectedActivityGroupIndex != -1;
        public Activity SelectedActivityGroup => IsSelectedActivityGroupIndex ? ActivitiesGroup[SelectedActivityGroupIndex] : null;


        public List<Activity> ActivitiesNotAssignedGroup { get; set; } 
        //=> IsGroupSelected && SetActivities ? _activityStore.Activities.Where(a => !ActivitiesGroup.Contains(a)).ToList() : null;

        private int _selectedActivityNotAssignedGroupIndex = -1;
        public int SelectedActivityNotAssignedGroupIndex
        {
            get
            {
                return _selectedActivityNotAssignedGroupIndex;
            }
            set
            {
                _selectedActivityNotAssignedGroupIndex = value;
                OnPropertyChanged(nameof(SelectedActivityNotAssignedGroupIndex));
                OnPropertyChanged(nameof(IsSelectedActivityNotAssignedGroupIndex));
                OnPropertyChanged(nameof(SelectedActivityNotAssignedGroupIndex));
            }
        }
        public bool IsSelectedActivityNotAssignedGroupIndex => SelectedActivityNotAssignedGroupIndex != -1;
        public Activity SelectedActivityNotAssignedGroup => IsSelectedActivityNotAssignedGroupIndex ? ActivitiesNotAssignedGroup[SelectedActivityNotAssignedGroupIndex] : null;


        public override void Dispose()
        {
            _groupStore.GroupsChange -= GroupStore_GroupsChange;
            _userStore.UsersChange -= UserStore_UsersChange;
            _groupStore.GroupsActivitiesChange -= GroupStore_GroupsActivitiesChange;
            base.Dispose();
        }
    }
}
