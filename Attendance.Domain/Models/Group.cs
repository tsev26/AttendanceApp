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
                     int priority, 
                     User superVisor) : base ()
        {
            Name = name;
            Priority = priority;
            SuperVisor = superVisor;
            Users = new List<User>();
        }

        public Group(Group group)
        {
            Name = group.Name;
            Priority = group.Priority;
            SuperVisor = group.SuperVisor;
            Users = group.Users;
        }

        public string Name { get; set; }
        public int Priority { get; set; }
        public User SuperVisor { get; set; }
        public List<User> Users { get; set; }
        public Obligation Obligation { get; set; }
    }
}
