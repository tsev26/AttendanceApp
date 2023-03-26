using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserHistoryViewModel : ViewModelBase
    {
        private readonly CurrentUser _currentUser;

        public UserHistoryViewModel(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

      
    }
}
