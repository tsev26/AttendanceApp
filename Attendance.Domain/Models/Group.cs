using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Group : DomainObject
    {
        public Group(
                     string name, 
                     User superVisor) : base ()
        {
            Name = name;
            Supervisor = superVisor;
            Obligation = new Obligation();
        }

        public Group(Group group)
        {
            Name = group.Name;
            Supervisor = group.Supervisor;
            Obligation = new Obligation();
        }

        public string Name { get; set; }
        public User Supervisor { get; set; }
        public Obligation Obligation { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Group Clone()
        {
            return new Group(this)
            {
                Id = this.Id,
                Obligation = this.Obligation.Clone(),
                Supervisor = this.Supervisor,
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Group other = (Group)obj;
            return Id == other.Id &&
                   Name == other.Name;
        }

        public static bool operator ==(Group a, Group b)
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

        public static bool operator !=(Group a, Group b)
        {
            return !(a == b);
        }
    }
}
