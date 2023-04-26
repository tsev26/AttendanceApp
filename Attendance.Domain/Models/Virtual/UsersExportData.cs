using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models.Virtual
{
    public class UsersExportData
    {
        public User User { get; set; }
        public DateOnly Date { get; set; }
        public bool ShouldWork { get; set; }
        public bool LatestArival { get; set; }
        public bool EarliestDeparture { get; set; }
        public List<ActivityExportData> ActivityExportDatas { get; set; }
    }
}
