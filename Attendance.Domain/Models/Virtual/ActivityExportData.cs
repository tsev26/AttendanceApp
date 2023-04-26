using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models.Virtual
{
    public class ActivityExportData
    {
        public string ActivityName { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
