using Attendance.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public NavigationBarViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
