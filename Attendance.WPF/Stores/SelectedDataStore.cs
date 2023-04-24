using Attendance.Domain.Models;
using Attendance.EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class SelectedDataStore
    {
        private readonly UserStore _userStore;
        private readonly UserDataService _userDataService;
        private readonly AttendanceRecordStore _attendanceRecordStore;

        public SelectedDataStore(UserStore userStore,  UserDataService userDataService, AttendanceRecordStore attendanceRecordStore)
        {
            _userStore = userStore;
            _userDataService = userDataService;
            _attendanceRecordStore = attendanceRecordStore;
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
            }
        }

        public Activity SelectedActivity { get; set; }

        public Key SelectedKeyValue { get; set; }

        public AttendanceRecord AttendanceRecord { get; set; }

        public event Action SelectedUserChange;
        public event Action SelectedUserAttendanceChange;

        private async Task LoadUserAttendances()
        {
            if (SelectedUser != null)
            {
                await _attendanceRecordStore.LoadAttendanceRecords(SelectedUser);

                await _attendanceRecordStore.LoadAttendanceTotals(SelectedUser);

                await _attendanceRecordStore.LoadAttendanceRecordFixes(SelectedUser);
            }
            else
            {
                _attendanceRecordStore.AttendanceRecords = new List<AttendanceRecord>();
                _attendanceRecordStore.AttendanceRecordFixes = new List<AttendanceRecordFix>();
                _attendanceRecordStore.AttendanceTotal = new List<AttendanceTotal>();
            }
            SelectedUserAttendanceChange?.Invoke();

        }

        public void SetGroup(User user, Group group)
        {
            user.Group = group;
            SelectedUserChange?.Invoke();
        }

        public void UpdateUser(User user)
        {
            int index = _userStore.Users.FindIndex(a => a.ID == user.ID);
            if (index != -1)
            {
                _userStore.Users[index] = user;
            }
            SelectedUserChange?.Invoke();
        }

        public async Task RemoveKey(User user, Key key)
        {
            await _userDataService.RemoveKey(key);
            user.Keys.Remove(key);
            SelectedUserChange?.Invoke();
        }

        public async Task<bool> UpsertKey(User user, Key newKeyValue)
        {
            bool change = false;
            change = await _userDataService.UpsertKey(user, newKeyValue);
            if (change)
            {
                Key? existingKey = user.Keys.FirstOrDefault(a => a.ID == newKeyValue.ID);
                if (existingKey != null)
                {
                    int index = user.Keys.IndexOf(existingKey);
                    user.Keys[index] = newKeyValue;
                }
                else
                {
                    user.Keys.Add(newKeyValue);
                }
            }
            SelectedUserChange?.Invoke();
            return change;
        }

        public async Task SetFastWork(bool isFastWorkSet)
        {
            await _userDataService.SetFastWork(SelectedUser, isFastWorkSet);
            SelectedUser.IsFastWorkSet = isFastWorkSet;
        }
    }
}
