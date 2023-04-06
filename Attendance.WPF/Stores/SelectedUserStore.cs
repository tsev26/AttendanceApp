using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class SelectedUserStore
    {
        private readonly UserStore _userStore;
        private readonly AttendanceRecordStore _attendanceRecordStore;
        public SelectedUserStore(UserStore userStore, AttendanceRecordStore attendanceRecordStore)
        {
            _userStore = userStore;
            _attendanceRecordStore = attendanceRecordStore;
        }

        public AttendanceRecordStore AttendanceRecordStore => _attendanceRecordStore;

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

        public void SetGroup(User user, Group group)
        {
            user.Group = group;
            SelectedUserChange?.Invoke();
        }

        public void UpdateUser(User user)
        {
            int index = _userStore.Users.FindIndex(a => a.Id == user.Id);
            if (index != -1)
            {
                _userStore.Users[index] = user;
            }
            SelectedUserChange?.Invoke();
        }

        public void RemoveKey(User user, Key key)
        {
            user.Keys.Remove(key);
            SelectedUserChange?.Invoke();
        }

        public void UpsertKey(User user, Key newKeyValue)
        {
            Key? existingKey = user.Keys.FirstOrDefault(a => a.Id == newKeyValue.Id);
            if (existingKey != null)
            {
                int index = user.Keys.IndexOf(existingKey);
                user.Keys[index] = newKeyValue;
            }
            else
            {
                user.Keys.Add(newKeyValue);
            }
            SelectedUserChange?.Invoke();
        }
    }
}
