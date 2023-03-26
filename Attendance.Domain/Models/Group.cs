﻿using System;
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
            Users = new List<User>();
            Obligation = new Obligation();
        }

        public Group(Group group)
        {
            Name = group.Name;
            Supervisor = group.Supervisor;
            Users = group.Users;
            Obligation = new Obligation();
        }

        public string Name { get; set; }
        public User Supervisor { get; set; }
        public List<User> Users { get; set; }
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
                Users = this.Users
            };
        }
    }
}
