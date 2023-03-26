using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Models
{
    public class MonthlyAttendanceTotalsWork
    {
        public DateOnly Date { get; set; }
        public string DateName { get; set; }
        public User User { get; set; }
        public TimeSpan Worked { get; set; }
    }
}
