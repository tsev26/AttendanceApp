using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class CurrentUser
    {
        private User _user;

        public CurrentUser()
        {

        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
