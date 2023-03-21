using Attendance.Domain.Models;
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
        public UserProfileViewModel(CurrentUser currentUser, UserStore userStore)
        {
            CurrentUser = currentUser;
            _userStore = userStore;

            UserUpdates = new ObservableCollection<User>();
            UpdatesOnUserCommand = new CreateUpdateOnUserCommand(currentUser, userStore);
            DeleteUpdateOnUserCommand = new DeleteUpdateOnUserCommand(currentUser, userStore);

            if (currentUser.User.Obligation == null)
            {
                ObligationFromUser = false;
            }
            else
            {
                ObligationFromUser = true;
            }

            SetCurrentUserUpdates();

            UserStore_UsersChange();
            CurrentUser.CurrentUserUpdatesChange += CurrentUser_CurrentUserUpdatesChange;
            _userStore.UsersChange += UserStore_UsersChange;

        }

        private void UserStore_UsersChange()
        {
            UserUpdates.Clear();
            foreach (var user in _userStore.Users.Where(a => a.UserId == CurrentUser.User.UserId && a.ToApprove == true))
            {
                UserUpdates.Add(user);
            }
        }

        private void SetCurrentUserUpdates()
        {
            if (SelectedIndex == -1)
            {
                CurrentUser.UserUpdates = new User(CurrentUser.User.UserId, CurrentUser.User.FirstName, CurrentUser.User.LastName, CurrentUser.User.Email, CurrentUser.User.IsAdmin, true);
                CurrentUser.UserUpdates.Obligation = new Obligation();
                SetUserUpdate(CurrentUser.User);
            }
            else
            {
                CurrentUser.UserUpdates = UserUpdates[SelectedIndex];
                SetUserUpdate(CurrentUser.UserUpdates);
            }

            CurrentUser.UserUpdates.Keys = CurrentUser.User.Keys;

            if (CurrentUser.User.Group != null)
            {
                CurrentUser.UserUpdates.Group = new Group(CurrentUser.User.Group);
            }
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
                OnPropertyChanged(nameof(UserUpdate));
                OnPropertyChanged(nameof(IsSelected));
                SetCurrentUserUpdates();
            }
        }

        public User UserUpdate => CurrentUser.UserUpdates;


        private void SetUserUpdate(User user)
        {
            if (user == null) return;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            if (user.Obligation == null)
            {
                HasRegularWorkingTime = user.Group.Obligation.HasRegularWorkingTime;
                MinHoursWorked = user.Group.Obligation.MinHoursWorked;
                LatestArival = user.Group.Obligation.LatestArival;
                EarliestDeparture = user.Group.Obligation.EarliestDeparture;

                WorksMonday = user.Group.Obligation.WorksMonday;
                WorksTuesday = user.Group.Obligation.WorksTuesday;
                WorksWednesday = user.Group.Obligation.WorksWednesday;
                WorksThursday = user.Group.Obligation.WorksThursday;
                WorksFriday = user.Group.Obligation.WorksFriday;
                WorksSaturday = user.Group.Obligation.WorksSaturday;
                WorksSunday = user.Group.Obligation.WorksSunday;
            }
            else
            {
                HasRegularWorkingTime = user.Obligation.HasRegularWorkingTime;
                MinHoursWorked = user.Obligation.MinHoursWorked;
                LatestArival = user.Obligation.LatestArival;
                EarliestDeparture = user.Obligation.EarliestDeparture;

                WorksMonday = user.Obligation.WorksMonday;
                WorksTuesday = user.Obligation.WorksTuesday;
                WorksWednesday = user.Obligation.WorksWednesday;
                WorksThursday = user.Obligation.WorksThursday;
                WorksFriday = user.Obligation.WorksFriday;
                WorksSaturday = user.Obligation.WorksSaturday;
                WorksSunday = user.Obligation.WorksSunday;
            }

        }

        private void CurrentUser_CurrentUserUpdatesChange()
        {
            OnPropertyChanged(nameof(UserUpdates));
        }

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
                CurrentUser.UserUpdates.FirstName = _firstName;
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
                CurrentUser.UserUpdates.LastName = _lastName;
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
                CurrentUser.UserUpdates.Email = _email;
                OnPropertyChanged(nameof(Email));
            }
        }

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
                CurrentUser.UserUpdates.Obligation.HasRegularWorkingTime = _hasRegularWorkingTime;
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
                CurrentUser.UserUpdates.Obligation.MinHoursWorked = _minHoursWorked;
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
                CurrentUser.UserUpdates.Obligation.LatestArival = _latestArival;
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
                CurrentUser.UserUpdates.Obligation.EarliestDeparture = _earliestDeparture;
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
                CurrentUser.UserUpdates.Obligation.WorksMonday = _worksMonday;
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
                CurrentUser.UserUpdates.Obligation.WorksTuesday = _worksTuesday;
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
                CurrentUser.UserUpdates.Obligation.WorksWednesday = _worksWednesday;
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
                CurrentUser.UserUpdates.Obligation.WorksThursday = _worksThursday;
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
                CurrentUser.UserUpdates.Obligation.WorksFriday = _worksFriday;
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
                CurrentUser.UserUpdates.Obligation.WorksSaturday = _worksSaturday;
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
                CurrentUser.UserUpdates.Obligation.WorksSunday = _worksSunday;
                OnPropertyChanged(nameof(WorksSunday));
            }
        }

        public CurrentUser CurrentUser { get; set; }

        public ObservableCollection<User> UserUpdates { get; set; }

        public ICommand UpdatesOnUserCommand { get; }
        public ICommand DeleteUpdateOnUserCommand { get; }

        public bool ObligationFromUser { get; }

        public string ObligationFrom => "(nastavení " + (ObligationFromUser ? "z uživatele" : "ze skupiny") + ")";

        public override void Dispose()
        { 
            CurrentUser.CurrentUserUpdatesChange -= CurrentUser_CurrentUserUpdatesChange;
            _userStore.UsersChange -= UserStore_UsersChange;
            CurrentUser.UserUpdates = null;
            base.Dispose();
        }
    }
}
