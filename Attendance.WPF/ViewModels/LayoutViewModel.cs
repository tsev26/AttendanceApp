using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class LayoutViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; }
        public ICommand NavigateHomeCommand { get; }

        public LayoutViewModel(INavigationService homeNavigationService, ViewModelBase contentViewModel)
        {
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            ContentViewModel.Dispose();
            base.Dispose();
        }
    }
}
