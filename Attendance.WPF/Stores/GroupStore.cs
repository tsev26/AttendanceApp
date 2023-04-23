using Attendance.Domain.Models;
using Attendance.EF.Services;
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
        private readonly UserDataService _userDataService;

        public GroupStore(UserDataService userDataService)
        {
            _groups = new List<Group>();
            _userDataService = userDataService;
        }

        public event Action GroupsChange;
        public event Action GroupsActivitiesChange;


        public async Task LoadGroups()
        {
            Groups = await _userDataService.GetGroups();
        }

        public List<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                GroupsChange?.Invoke();
            }
        }

        public async Task AddGroup(Group newGroup)
        {
            if (!_groups.Any(a => a.Name == newGroup.Name))
            {
                newGroup.Members = new List<User>();
                await _userDataService.AddGroups(newGroup);
                _groups.Add(newGroup);
            }
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

        public async Task RemoveGroup(Group group)
        {
            await _userDataService.RemoveGroups(group);
            LoadGroups();
        }

        public async Task SetSupervisor(User? user, Group group)
        {
            await _userDataService.SetSupervisorToGroup(user, group);
            group.Supervisor = user;
            GroupsChange?.Invoke();
        }

        public async Task UpdateGroupObligation(Group group)
        {
            await _userDataService.UpdateGroup(group);

            int index = _groups.FindIndex(a => a.ID == group.ID);

            if (index != -1)
            {
                _groups[index] = group;
            }
            GroupsChange?.Invoke();
        }

        public async Task RemoveActivityFromGroup(Group group, Activity activity)
        {
            await _userDataService.RemoveActivityFromGroup(group, activity);
            group.AvailableActivities.Remove(activity);
            GroupsActivitiesChange?.Invoke();
        }

        public async Task AddActivityToGroup(Group group, Activity activity)
        {
            await _userDataService.AddActivityToGroup(group, activity);
            group.AvailableActivities.Add(activity);
            GroupsActivitiesChange?.Invoke();
        }
    }
}
