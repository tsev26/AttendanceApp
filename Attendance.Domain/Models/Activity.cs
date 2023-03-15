using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Activity : DomainObject
    {
        public Activity(string name, string shortcut, ActivityProperty property)
        {
            Name = name;
            Shortcut = shortcut;
            Property = property;
        }

        public string Name { get; set; }
        public string Shortcut { get; set; }
        public ActivityProperty Property { get; set; }
    }
}
