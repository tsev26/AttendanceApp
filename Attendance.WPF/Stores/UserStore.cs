using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class UserStore
    {
        private IList<User> _users;

        public UserStore()
        {
            _users = new List<User>();
        }

        public event Action UsersChange;

        public IList<User> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
            }
        }

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
        }
    }
}
