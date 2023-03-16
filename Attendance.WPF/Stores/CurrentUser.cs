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
        private List<AttendanceRecord> _attendanceRecords;

        public CurrentUser()
        {
            _attendanceRecords = new List<AttendanceRecord>();
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public void SetActivity(Activity activity)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(_user, activity, DateTime.Now);
            _attendanceRecords.Add(attendanceRecord);
        }
    }
}
