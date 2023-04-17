﻿using Attendance.Domain.Models;
using Attendance.EF.Services;
using Attendance.WPF.Model;
using Attendance.WPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.XPath;
using static Attendance.WPF.Functions.TimeFunction;

namespace Attendance.WPF.Stores
{
    public class CurrentUserStore
    {
        private readonly UserStore _userStore;
        private readonly UserDataService _userDataService;
        public CurrentUserStore(UserStore userStore, UserDataService userDataService)
        {
            _userStore = userStore;
            _userDataService = userDataService;
        }

        public event Action CurrentUserChange;
        public event Action CurrentAttendanceChange;
        public event Action CurrentUserKeysChange;

        private User? _user;
        public User? User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                CurrentUserChange?.Invoke();
            }
        }

        public bool IsUserSuperVisor { get; private set; }

        public List<User> SubordinateUsers => _userStore.Users.Where(a => a.IsSubordinate(User)).ToList();

        public async Task LoadUser(User user)
        {
            IsUserSuperVisor = await _userDataService.IsSupervisor(user);
            User = await _userDataService.LoadUserData(user); 
        }

        public void Clear()
        {
            User = null;
        }

        public void RemoveKey(Key selectedKey)
        {
            User.Keys.Remove(selectedKey);
            CurrentUserKeysChange?.Invoke();
        }

        public void UpsertKey(Key newKeyValue)
        {
            Key? existingKey = User.Keys.FirstOrDefault(a => a.ID == newKeyValue.ID);
            if (existingKey != null)
            {
                int index = User.Keys.IndexOf(existingKey);
                User.Keys[index] = newKeyValue;
            }
            else
            {
                User.Keys.Add(newKeyValue);
            }
            CurrentUserKeysChange?.Invoke();
        }

    }
}
