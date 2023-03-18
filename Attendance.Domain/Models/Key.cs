using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Key : DomainObject
    {
        public Key(string keyValue) : base()
        {
            KeyValue = keyValue;
        }

        public string KeyValue { get; set; }
    }
}
