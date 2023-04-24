using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserHistoryOverviewViewModel : ViewModelBase
    {
        public UserHistoryOverviewViewModel(UserHistoryViewModel userHistoryViewModel, UserDailyOverviewViewModel userDailyOverviewViewModel)
        {
            UserHistoryViewModel = userHistoryViewModel;
            UserDailyOverviewViewModel = userDailyOverviewViewModel;
        }

        public UserHistoryViewModel UserHistoryViewModel { get; private set; }
        public UserDailyOverviewViewModel UserDailyOverviewViewModel { get; private set; }
    }
}
