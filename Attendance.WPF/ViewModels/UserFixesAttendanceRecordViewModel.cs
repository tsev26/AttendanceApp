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
    public class UserFixesAttendanceRecordViewModel : ViewModelBase
    {
        private readonly CurrentUserStore _currentUser;
		private readonly AttendanceRecordStore _attendanceRecordStore;

        public UserFixesAttendanceRecordViewModel(CurrentUserStore currentUser, SelectedDataStore selectedUserStore, AttendanceRecordStore attendanceRecordStore, MessageStore messageStore, INavigationService navigateFixAttendance)
        {
            _currentUser = currentUser;
			_attendanceRecordStore = attendanceRecordStore;

            NavigateFixAttendaceCommand = new NavigateFixAttendaceCommand(selectedUserStore, this, _attendanceRecordStore, messageStore, navigateFixAttendance);
			DeleteAttendanceRecordFixCommand = new DeleteAttendanceRecordFixCommand(currentUser, _attendanceRecordStore, messageStore, this);

            _attendanceRecordStore.CurrentAttendanceRecordFixChange += AttendanceRecordStore_CurrentAttendanceRecordFixChange;
        }

		public CurrentUserStore CurrentUser => _currentUser;

		public ICommand NavigateFixAttendaceCommand { get; }
		public ICommand DeleteAttendanceRecordFixCommand { get; }


        private void AttendanceRecordStore_CurrentAttendanceRecordFixChange()
		{
			OnPropertyChanged(nameof(AttendanceRecordFixes));
            OnPropertyChanged(nameof(AttendanceRecords));
        }

		public List<AttendanceRecordFix> AttendanceRecordFixes => _attendanceRecordStore.AttendanceRecordFixes(_currentUser.User).OrderBy(a => a.Approved).ToList();

        public List<AttendanceRecord> AttendanceRecords => _attendanceRecordStore.AttendanceRecords(_currentUser.User).OrderByDescending(a => a.Entry).ToList();

		private int _selectedAttendanceRecordFixIndex = -1;
		public int SelectedAttendanceRecordFixIndex
        {
			get
			{
				return _selectedAttendanceRecordFixIndex;
			}
			set
			{
				_selectedAttendanceRecordFixIndex = value;
				OnPropertyChanged(nameof(SelectedAttendanceRecordFixIndex));
                OnPropertyChanged(nameof(IsSelectedAttendanceRecordFix));
                OnPropertyChanged(nameof(AttendanceRecordFix));
                OnPropertyChanged(nameof(IsSelectedAttendanceRecordFixWainting));
            }
		}

        public bool IsSelectedAttendanceRecordFix => _selectedAttendanceRecordFixIndex != -1;
        public AttendanceRecordFix SelectedAttendanceRecordFix => IsSelectedAttendanceRecordFix ? AttendanceRecordFixes[SelectedAttendanceRecordFixIndex] : null;
        public bool IsSelectedAttendanceRecordFixWainting => IsSelectedAttendanceRecordFix && SelectedAttendanceRecordFix.Approved == ApproveType.Waiting;

		private int _selectedAttendanceRecordIndex = -1;
		public int SelectedAttendanceRecordIndex
        {
			get
			{
				return _selectedAttendanceRecordIndex;
			}
			set
			{
				_selectedAttendanceRecordIndex = value;
				OnPropertyChanged(nameof(SelectedAttendanceRecordIndex));
                OnPropertyChanged(nameof(IsSelectedAttendanceRecord));
                OnPropertyChanged(nameof(SelectedAttendanceRecord));
            }
		}

        public bool IsSelectedAttendanceRecord => _selectedAttendanceRecordIndex != -1;
        public AttendanceRecord SelectedAttendanceRecord => IsSelectedAttendanceRecord ? AttendanceRecords[SelectedAttendanceRecordIndex] : null;

		public override void Dispose()
		{
            _attendanceRecordStore.CurrentAttendanceRecordFixChange -= AttendanceRecordStore_CurrentAttendanceRecordFixChange;
            base.Dispose();
		}
	}
}
