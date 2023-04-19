using Attendance.Domain.Models;
using Attendance.EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class UserStore
    {
        private List<User> _users;
        private readonly UserDataService _userDataService;

        public UserStore(UserDataService userDataService)
        {
            _users = new List<User>();
            _userDataService = userDataService;
        }

        public event Action UsersChange;

        public List<User> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
            }
        }

        public AttendanceRecordStore AttendanceRecordStore { get; }

        public void AddUser(User newUser)
        {
            if (!_users.Contains(newUser))
            {
                _users.Add(newUser);
            }
            UsersChange?.Invoke();
        }

        public void DeleteUser(User deleteUser)
        {
            _users.Remove(deleteUser);
            UsersChange?.Invoke();
        }

        public void SetGroup(User user, Group group)
        {
            user.Group = group;
            UsersChange?.Invoke();
        }

        public void UpdateUser(User user)
        {
            int index = _users.FindIndex(a => a.UserId == user.UserId);
            if (index != -1)
            {
                user.ToApprove = false;
                _users[index] = user;
            }
            UsersChange?.Invoke();
        }

        public void RemoveKey(User user, Key key)
        {
            user.Keys.Remove(key);
            UsersChange?.Invoke();
        }

        public void UpsertKey(User user, Key newKeyValue)
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
            UsersChange?.Invoke();
        }

        public List<User> GetPendingUpdates(User? user)
        {
            List<User> records = new List<User>();

            records.AddRange(Users.Where(a => a.ToApprove && (a.IsSubordinate(user))));

            return records;
        }

        public async Task<User> GetUserByKey(string key)
        {
            return await _userDataService.GetUserByKey(key);
        }

        public User GetUserByUserId(int userId)
        {
            return Users.FirstOrDefault(a => a.UserId == userId && !a.ToApprove);
        }
    }
}
