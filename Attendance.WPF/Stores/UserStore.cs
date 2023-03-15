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
            _users.Add(newUser);
        }
    }
}
