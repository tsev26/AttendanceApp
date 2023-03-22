using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class GroupStore
    {
        private IList<Group> _groups;

        public GroupStore()
        {
            _groups = new List<Group>();
        }

        public event Action GroupsChange;

        public IList<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
            }
        }

        public void AddGroup(Group newGroup)
        {
            if (!_groups.Contains(newGroup))
            {
                _groups.Add(newGroup);
            }
            GroupsChange?.Invoke();
        }

        public void DeleteUser(Group deleteGroup)
        {
            _groups.Remove(deleteGroup);
            GroupsChange?.Invoke();
        }

        public void AddUserToGroup(Group group, User user)
        {
            if (!group.Users.Contains(user))
            {
                group.Users.Add(user);
            }
            GroupsChange?.Invoke();
        }

        public void RemoveUserToGroup(Group group, User user)
        {
            group.Users.Remove(user);
            GroupsChange?.Invoke();
        }
    }
}
