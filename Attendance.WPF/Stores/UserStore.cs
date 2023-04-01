﻿using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class UserStore
    {
        private List<User> _users;

        public UserStore()
        {
            _users = new List<User>();
        }

        public event Action UsersChange;

        public List<User> Users
        {
            get { return _users; }
            set 
            { 
                _users = value;
            }
        }

        public void AddUser(User newUser)
        {
            if (!_users.Contains(newUser))
            {
                _users.Add(newUser);
            }
            UsersChange?.Invoke();
        }

        public void DeleteUser(User deleteUser)
        {
            _users.Remove(deleteUser);
            UsersChange?.Invoke();
        }

        public void SetGroup(User user, Group group)
        {
            user.Group = group;
            UsersChange?.Invoke();
        }

        public void UpdateUser(User user)
        {
            int index = _users.FindIndex(a => a.Id == user.Id);
            if (index != -1)
            {
                _users[index] = user;
            }
            UsersChange?.Invoke();
        }

        public void RemoveKey(User user, Key key)
        {
            user.Keys.Remove(key);
            UsersChange?.Invoke();
        }

        public void UpsertKey(User user, Key newKeyValue)
        {

            Key? existingKey = user.Keys.FirstOrDefault(a => a.Id == newKeyValue.Id);
            if (existingKey != null)
            {
                int index = user.Keys.IndexOf(existingKey);
                user.Keys[index] = newKeyValue;
            }
            else
            {
                user.Keys.Add(newKeyValue);
            }
            UsersChange?.Invoke();
        }
    }
}
