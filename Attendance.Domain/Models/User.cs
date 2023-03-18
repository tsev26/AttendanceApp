using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Attendance.Domain.Models
{
    public class User : DomainObject
    {
        public User(
                    string firstName, 
                    string lastName, 
                    string email, 
                    bool isAdmin) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsAdmin = isAdmin;
            Groups = new List<Group>();
            Keys = new List<Key>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public List<Group> Groups { get; set; }
        public List<Key> Keys { get; set; }
        public Obligation? Obligation { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            User other = (User)obj;
            return Id == other.Id && FirstName == other.FirstName && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (FirstName.GetHashCode() + LastName.GetHashCode());
        }

        public static bool operator ==(User a, User b)
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

        public static bool operator !=(User a, User b)
        {
            return !(a == b);
        }
    }
}
