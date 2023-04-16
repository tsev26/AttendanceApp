using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Key(string keyValue, User user) : base()
        {
            KeyValue = keyValue;
            User = user;
            UserId = user.ID;
        }

        public string KeyValue { get; set; }

        
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
        

        public Key Clone()
        {
            return new Key(KeyValue)
            {
                ID = this.ID
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
