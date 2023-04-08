using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UsersRequestsViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly UserStore _userStore;

        public UsersRequestsViewModel(CurrentUserStore currentUser, AttendanceRecordStore attendanceRecordStore, UserStore userStore)
        {
            _currentUser = currentUser;
            _attendanceRecordStore = attendanceRecordStore;
            _userStore = userStore;

            ProfileDecisionCommand = new ProfileDecisionCommand(userStore, this);
            FixDecisionCommand = new FixDecisionCommand(attendanceRecordStore, this);

            _userStore.UsersChange += UserStore_UsersChange;
            _attendanceRecordStore.CurrentAttendanceRecordFixChange += AttendanceRecordStore_CurrentAttendanceRecordFixChange;
        }

        private void AttendanceRecordStore_CurrentAttendanceRecordFixChange()
        {
            OnPropertyChanged(nameof(PendingRequestFixes));
        }

        private void UserStore_UsersChange()
        {
            OnPropertyChanged(nameof(PendingProfileUpdates));
        }

        public ICommand FixDecisionCommand { get; }
        public ICommand ProfileDecisionCommand { get; }

        public List<AttendanceRecordFix> PendingRequestFixes => _attendanceRecordStore.GetPendingFixes(_currentUser.User);

        private int _selectedPendingRequestFixIndex = -1;
        public int SelectedPendingRequestFixIndex
        {
            get
            {
                return _selectedPendingRequestFixIndex;
            }
            set
            {
                _selectedPendingRequestFixIndex = value;
                OnPropertyChanged(nameof(SelectedPendingRequestFixIndex));
                OnPropertyChanged(nameof(IsSelectedPendingRequestFix));
                OnPropertyChanged(nameof(SelectedPendingRequestFix));
                OnPropertyChanged(nameof(SelectedAttendanceRecord));
                OnPropertyChanged(nameof(IsSelectedAttendanceRecord));
                if (SelectedPendingRequestFixIndex != -1)
                {
                    SelectedPendingProfileUpdateIndex = -1;
                    OnPropertyChanged(nameof(SelectedPendingProfileUpdateIndex));
                }
            }
        }

        public bool IsSelectedPendingRequestFix => SelectedPendingRequestFixIndex != -1;
        public AttendanceRecordFix SelectedPendingRequestFix => IsSelectedPendingRequestFix ? PendingRequestFixes[SelectedPendingRequestFixIndex] : null;
        public AttendanceRecord SelectedAttendanceRecord => IsSelectedPendingRequestFix ? SelectedPendingRequestFix.AttendanceRecord : null;
        public bool IsSelectedAttendanceRecord => SelectedAttendanceRecord != null;

        public List<User> PendingProfileUpdates => _userStore.GetPendingUpdates(_currentUser.User);

        private int _selectedPendingProfileUpdateIndex = -1;
        public int SelectedPendingProfileUpdateIndex
        {
            get
            {
                return _selectedPendingProfileUpdateIndex;
            }
            set
            {
                _selectedPendingProfileUpdateIndex = value;
                OnPropertyChanged(nameof(SelectedPendingProfileUpdateIndex));
                OnPropertyChanged(nameof(IsSelectedPendingProfileUpdate));
                OnPropertyChanged(nameof(SelectedPendingProfileUpdate));
                OnPropertyChanged(nameof(SelectedPendingProfileNow));
                if (SelectedPendingProfileUpdateIndex != -1)
                {
                    SelectedPendingRequestFixIndex = -1;
                    OnPropertyChanged(nameof(SelectedPendingRequestFixIndex));
                }
            }
        }

        public bool IsSelectedPendingProfileUpdate => SelectedPendingProfileUpdateIndex != -1;
        public User SelectedPendingProfileUpdate => IsSelectedPendingProfileUpdate ? PendingProfileUpdates[SelectedPendingProfileUpdateIndex] : null;
        public User SelectedPendingProfileNow => IsSelectedPendingProfileUpdate ? _userStore.GetUserByUserId(SelectedPendingProfileUpdate.UserId) : null;


        public override void Dispose()
        {
            _userStore.UsersChange -= UserStore_UsersChange;
            base.Dispose();
        }
    }
}
