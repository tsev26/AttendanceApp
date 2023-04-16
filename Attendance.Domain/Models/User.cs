using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Attendance.Domain.Models
{
    public class User : DomainObject
    {
        private static int _nextId = 1;

        public User() : base() { }
        public User(
                    string firstName,
                    string lastName,
                    string email,
                    bool isAdmin = false,
                    bool toApprove = false) : base()
        {
            UserId = _nextId++;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsAdmin = isAdmin;
            ToApprove = toApprove;
            IsFastWorkSet = false;
            Keys = new List<Key>();
        }

        public User(
                    int userId,
                    string firstName,
                    string lastName,
                    string email,
                    bool isAdmin = false,
                    bool toApprove = false) : base()
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsAdmin = isAdmin;
            ToApprove = toApprove;
            IsFastWorkSet = false;
            Keys = new List<Key>();
        }

        public User(User user) : base()
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            IsAdmin = user.IsAdmin;
            ToApprove = user.ToApprove;
            Keys = user.Keys;
            Group = user.Group;
            IsFastWorkSet = user.IsFastWorkSet;
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFastWorkSet { get; set; }
        public bool ToApprove { get; set; }

        
        public Group Group { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        [InverseProperty("User")]
        public List<Key> Keys { get; set; }

        
        public Obligation? Obligation { get; set; }
        [ForeignKey("Obligation")]
        public int? ObligationId { get; set; }


        [NotMapped]
        public bool HasObligation => Obligation != null;

        [NotMapped]
        public Obligation UserObligation => HasObligation ? Obligation : Group?.Obligation ?? null;

        [NotMapped]
        public string HasObligationString => HasObligation ? "(nastavení z uživatele)" : "(nastavení ze skupiny)";

        public void AddKey(string keyValue)
        {
            Key newKey = new Key(keyValue, this);
            Keys.Add(newKey);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            User other = (User)obj;
            return UserId == other.UserId && 
                   FirstName == other.FirstName && 
                   LastName == other.LastName &&
                   Email == other.Email &&
                   ToApprove == other.ToApprove &&
                   Obligation?.HasRegularWorkingTime == other.Obligation?.HasRegularWorkingTime &&
                   Obligation?.MinHoursWorked == other.Obligation?.MinHoursWorked &&
                   Obligation?.LatestArival == other.Obligation?.LatestArival &&
                   Obligation?.EarliestDeparture == other.Obligation?.EarliestDeparture &&
                   Obligation?.WorksMonday == other.Obligation?.WorksMonday &&
                   Obligation?.WorksTuesday == other.Obligation?.WorksTuesday &&
                   Obligation?.WorksWednesday == other.Obligation?.WorksWednesday &&
                   Obligation?.WorksThursday == other.Obligation?.WorksThursday &&
                   Obligation?.WorksFriday == other.Obligation?.WorksFriday &&
                   Obligation?.WorksSaturday == other.Obligation?.WorksSaturday &&
                   Obligation?.WorksSunday == other.Obligation?.WorksSunday;
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


        public User Clone()
        {
            return new User(this)
            {
                ID = this.ID,
                Obligation = Obligation?.Clone()
            };
        }

        public bool IsSubordinate(User user)
        {
            return Group.Supervisor == user || user.IsAdmin;
        }

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
    }
}
