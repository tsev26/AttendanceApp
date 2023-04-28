using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models.Virtual
{
    public class UsersExportData
    {
        public string UserName { get; set; }
        public DateOnly Date { get; set; }
        public List<ActivityExportData> ActivityExportDatas { get; set; }
    }
}
