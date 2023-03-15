using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserMenuViewModel : ViewModelBase
    {
        private readonly UserDailyOverviewViewModel _userDailyOverviewViewModel;
        private readonly UserSelectActivityViewModel _userSelectActivityViewModel;
        private readonly CurrentUser _currentUser;
        private readonly ActivityStore _activityStore;

        public UserMenuViewModel(UserDailyOverviewViewModel userDailyOverviewViewModel, 
                                 UserSelectActivityViewModel userSelectActivityViewModel, 
                                 CurrentUser currentUser, 
                                 ActivityStore activityStore)
        {
            _userDailyOverviewViewModel = userDailyOverviewViewModel;
            _userSelectActivityViewModel = userSelectActivityViewModel;
            _currentUser = currentUser;
            _activityStore = activityStore;
        }

    }
}
