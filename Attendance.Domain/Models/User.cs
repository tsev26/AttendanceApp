using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class User : DomainObject
    {
        public User(string firstName, 
                    string lastName, 
                    string email, 
                    bool isAdmin)
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
    }
}
