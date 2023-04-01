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

        public Key Clone()
        {
            return new Key(KeyValue)
            {
                Id = this.Id
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Key other = (Key)obj;
            return KeyValue == other.KeyValue;
        }

        /*
        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ KeyValue.GetHashCode();
        }
        */


        public static bool operator ==(Key a, Key b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Key a, Key b)
        {
            return !(a == b);
        }
    }
}
