using Attendance.Domain.Models;
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
        public SelectedDataStore(UserStore userStore, AttendanceRecordStore attendanceRecordStore)
        {
            _userStore = userStore;
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

        public bool UpsertKey(User user, Key newKeyValue)
        {
            bool change = false;
            if (_userStore.Users.Exists(a => a.Keys.Contains(newKeyValue)))
            {
                return change;
            }
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
            return change;
        }

        public void SetFastWork(bool isFastWorkSet)
        {
            SelectedUser.IsFastWorkSet = isFastWorkSet;
        }
    }
}
