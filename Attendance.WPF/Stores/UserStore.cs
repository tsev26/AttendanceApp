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

        public async Task LoadUsers(User? user = null)
        {
            Users = await _userDataService.GetUsers(user);
        }

        public List<User> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
            }
        }

        public AttendanceRecordStore AttendanceRecordStore { get; }

        public async Task AddUser(User newUser)
        {
            Group group = newUser.Group;
            User user = await _userDataService.AddUser(newUser);
            
            
            if (user != null)
            {
                user.Group = group;
                _users.Add(user);
            }    
            

            UsersChange?.Invoke();
        }

        public async Task<List<User>> LoadFixProfile(User user)
        {
            return await _userDataService.LoadUserProfileFixes(user);
        }

        public async Task DeleteUser(User deleteUser)
        {
            await _userDataService.RemoveUser(deleteUser);
            _users.Remove(deleteUser);
            UsersChange?.Invoke();
        }

        public async Task SetGroup(User user, Group group)
        {
            await _userDataService.UserSetGroup(user, group);
            user.Group = group;
            group.Members.Add(user);
            UsersChange?.Invoke();
        }

        public async Task UpdateUser(User user)
        {
            await _userDataService.UpdateUser(user);
            
            
            int index = _users.FindIndex(a => a.ID == user.ID);
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

        public async Task<List<User>> GetPendingUpdates(User? user)
        {
            //List<User> records = new List<User>();
            //records.AddRange(Users.Where(a => a.ToApprove && (a.IsSubordinate(user))));
            return await _userDataService.GetPendingProfileUpdates(user);
        }

        public async Task<User> GetUserByKey(string key)
        {
            return await _userDataService.GetUserByKey(key);
        }

        public async Task<User> GetUserByUserId(int userId)
        {
            User user = await _userDataService.GetUserByUserId(userId);
            return user;
        }
    }
}
