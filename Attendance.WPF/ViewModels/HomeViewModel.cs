﻿using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
		private UserStore _userStore;
		private INavigationService _navigateToUserMenu;
		private CurrentUser _currentUser;
        public HomeViewModel(UserStore userStore,
                             INavigationService navigateToUserMenu,
							 CurrentUser currentUser)
        {
            _userStore = userStore;
			_currentUser = currentUser;
            _navigateToUserMenu = navigateToUserMenu;
            ClearUserKeyCommand = new ClearUserKeyCommand(this);
			NavigateTo = new NavigateCommand(navigateToUserMenu);
        }

        public ICommand ClearUserKeyCommand { get; set; }
		public ICommand NavigateTo { get; set; }

        private string _userKey;
		public string UserKey
		{
			get
			{
				return _userKey;
			}
			set
			{
				_userKey = value;
				OnPropertyChanged(nameof(UserKey));
                OnPropertyChanged(nameof(HasText));
                if (_userKey.Length >= 3)
				{
                    CheckIfKeyIsUsers(UserKey);
                }
            }
		}

		public bool HasText => (_userKey??"").Length > 0;


        public void CheckIfKeyIsUsers(string key)
		{
			User? user = _userStore.Users.FirstOrDefault(a => a.Keys.Any(c => c.KeyValue.ToUpper() == key));

            if (user != null)
            {
				_currentUser.User = user;
                _currentUser.LoadUser(user);
				_userKey = "";
				_navigateToUserMenu.Navigate();
            }

            /*
			foreach (User user in _userStore.Users)
			{
				foreach(Key userKey in user.Keys)
				{
					if(userKey.KeyValue == key)
					{
						_currentUser.User = user;
                        _navigateToUserMenu.Navigate();
                        return;
                    }
				}
			}
			*/
        }
	}
}
