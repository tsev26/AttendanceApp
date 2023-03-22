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

        public GroupsViewModel(GroupStore groupStore, 
                               INavigationService navigateAddGroup, 
                               INavigationService navigateAddUserToGroup)
        {
            _groupStore = groupStore;

            Groups = new ObservableCollection<Group>();

            NavigateCreateGroupCommand = new NavigateCommand(navigateAddGroup);
            DeleteGroupCommand = new DeleteGroupCommand(_groupStore);
            NavigateAddUser = new NavigateCommand(navigateAddUserToGroup);
            RemoveUserFromGroup = new RemoveUserFromGroupCommand(_groupStore);
            SaveGroupChanges = new SaveGroupChangesCommand(_groupStore);
            ChangeSupervisorOfGroupCommand = new ChangeSupervisorOfGroupCommand(_groupStore);

            _groupStore.GroupsChange += GroupStore_GroupsChange;

            GroupStore_GroupsChange();
        }

        private void GroupStore_GroupsChange()
        {
            Groups.Clear();
            foreach (var group in _groupStore.Groups.ToList())
            {
                Groups.Add(group);
            }
        }

        private void GroupSelectionChange()
        {
            Groups.Clear();
            foreach (var group in _groupStore.Groups.ToList())
            {
                Groups.Add(group);
            }
        }

        public ICommand NavigateCreateGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand NavigateAddUser { get; }
        public ICommand RemoveUserFromGroup { get; }
        public ICommand SaveGroupChanges { get; }
        public ICommand ChangeSupervisorOfGroupCommand { get; }



        public ObservableCollection<Group> Groups { get; set; }

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
            }
        }

        public bool IsUserSelected => SelectedUserIndex != -1;

        private bool _hasRegularWorkingTime;
        public bool HasRegularWorkingTime
        {
            get
            {
                return _hasRegularWorkingTime;
            }
            set
            {
                _hasRegularWorkingTime = value;
                OnPropertyChanged(nameof(HasRegularWorkingTime));
            }
        }

        private double _minHoursWorked;
        public double MinHoursWorked
        {
            get
            {
                return _minHoursWorked;
            }
            set
            {
                _minHoursWorked = value;
                OnPropertyChanged(nameof(MinHoursWorked));
            }
        }

        private TimeOnly _latestArival;
        public TimeOnly LatestArival
        {
            get
            {
                return _latestArival;
            }
            set
            {
                _latestArival = value;
                OnPropertyChanged(nameof(LatestArival));
            }
        }

        private TimeOnly _earliestDeparture;
        public TimeOnly EarliestDeparture
        {
            get
            {
                return _earliestDeparture;
            }
            set
            {
                _earliestDeparture = value;
                OnPropertyChanged(nameof(EarliestDeparture));
            }
        }

        private bool _worksMonday;
        public bool WorksMonday
        {
            get
            {
                return _worksMonday;
            }
            set
            {
                _worksMonday = value;
                OnPropertyChanged(nameof(WorksMonday));
            }
        }

        private bool _worksTuesday;
        public bool WorksTuesday
        {
            get
            {
                return _worksTuesday;
            }
            set
            {
                _worksTuesday = value;
                OnPropertyChanged(nameof(WorksTuesday));
            }
        }


        private bool _worksWednesday;
        public bool WorksWednesday
        {
            get
            {
                return _worksWednesday;
            }
            set
            {
                _worksWednesday = value;
                OnPropertyChanged(nameof(WorksWednesday));
            }
        }


        private bool _worksThursday;
        public bool WorksThursday
        {
            get
            {
                return _worksThursday;
            }
            set
            {
                _worksThursday = value;
                OnPropertyChanged(nameof(WorksThursday));
            }
        }

        private bool _worksFriday;
        public bool WorksFriday
        {
            get
            {
                return _worksFriday;
            }
            set
            {
                _worksFriday = value;
                OnPropertyChanged(nameof(WorksFriday));
            }
        }

        private bool _worksSaturday;
        public bool WorksSaturday
        {
            get
            {
                return _worksSaturday;
            }
            set
            {
                _worksSaturday = value;
                OnPropertyChanged(nameof(WorksSaturday));
            }
        }

        private bool _worksSunday;
        public bool WorksSunday
        {
            get
            {
                return _worksSunday;
            }
            set
            {
                _worksSunday = value;
                OnPropertyChanged(nameof(WorksSunday));
            }
        }
    }
}
