using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.Stores
{
    public class GroupStore
    {
        private List<Group> _groups;

        public GroupStore()
        {
            _groups = new List<Group>();
        }

        public event Action GroupsChange;
        public event Action GroupsActivitiesChange;

        public List<Group> Groups
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

        /*
        public void AddUserToGroup(Group group, User user)
        {
            Group? existingGroup = _groups.FirstOrDefault(a => a.Users.Any(c => c == user));
            if (existingGroup != null)
            {
                existingGroup.Users.Remove(user);
            }
            if (!group.Users.Contains(user))
            {
                group.Users.Add(user);
            }
            user.Group = group;
            GroupsChange?.Invoke();
        }
        */

        /*
        public void RemoveUserToGroup(Group group, User user)
        {
            group.Users.Remove(user);
            user.Group = null;
            GroupsChange?.Invoke();
        }
        */

        public void RemoveGroup(Group group)
        {
            _groups.Remove(group);
            GroupsChange?.Invoke();
        }

        public void SetSupervisor(User? user, Group group)
        {
            group.Supervisor = user;
            GroupsChange?.Invoke();
        }

        public void UpdateGroupObligation(Group group)
        {
            int index = _groups.FindIndex(a => a.ID == group.ID);

            if (index != -1)
            {
                _groups[index] = group;
            }
            GroupsChange?.Invoke();
        }

        public void RemoveActivityFromGroup(Group group, Activity activity)
        {
            group.Obligation.AvailableActivities.Remove(activity);
            GroupsActivitiesChange?.Invoke();
        }

        public void AddActivityToGroup(Group group, Activity activity)
        {
            group.Obligation.AvailableActivities.Add(activity);
            GroupsActivitiesChange?.Invoke();
        }
    }
}
