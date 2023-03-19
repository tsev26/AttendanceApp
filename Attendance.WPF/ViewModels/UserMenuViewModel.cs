﻿using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserMenuViewModel : ViewModelBase
    {
        private readonly CurrentUser _currentUser;

        public UserMenuViewModel(UserSelectActivityViewModel userSelectActivityViewModel,
                                 UserDailyOverviewViewModel userDailyOverviewViewModel,
                                 CurrentUser currentUser)
        {
            UserDailyOverviewViewModel = userDailyOverviewViewModel;
            UserSelectActivityViewModel = userSelectActivityViewModel;

            _currentUser = currentUser;
        }

        public UserDailyOverviewViewModel UserDailyOverviewViewModel { get; }
        public UserSelectActivityViewModel UserSelectActivityViewModel { get; }

        public override void Dispose()
        {
            UserDailyOverviewViewModel.Dispose();
            UserSelectActivityViewModel.Dispose();
            base.Dispose();
        }
    }
}
