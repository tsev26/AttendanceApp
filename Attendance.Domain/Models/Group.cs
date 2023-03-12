using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Group : DomainObject
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public User SuperVisor { get; set; }
        public List<User> Users { get; set; }
        public Obligation? Obligation { get; set; }
    }
}
