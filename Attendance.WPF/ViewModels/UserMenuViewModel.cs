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
        private readonly ActivityStore _activityStore;

        public UserMenuViewModel(
                                 UserSelectActivityViewModel userSelectActivityViewModel,
                                 UserDailyOverviewViewModel userDailyOverviewViewModel,
                                 CurrentUser currentUser, 
                                 ActivityStore activityStore)
        {
            UserDailyOverviewViewModel = userDailyOverviewViewModel;
            UserSelectActivityViewModel = userSelectActivityViewModel;
            _currentUser = currentUser;
            _activityStore = activityStore;
        }


        public UserDailyOverviewViewModel UserDailyOverviewViewModel { get; }
        public UserSelectActivityViewModel UserSelectActivityViewModel { get; }

        public override void Dispose()
        {
            _currentUser.Clear();
            UserDailyOverviewViewModel.Dispose();
            UserSelectActivityViewModel.Dispose();
            base.Dispose();
        }
    }
}