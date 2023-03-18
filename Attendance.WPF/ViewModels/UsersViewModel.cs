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
    public class UsersViewModel : ViewModelBase
    {
        public UsersViewModel(INavigationService navigateToHome)
        {
            NavigateHomeCommand = new NavigateCommand(navigateToHome);
        }

        public ICommand NavigateHomeCommand { get; set; }
    }
}
