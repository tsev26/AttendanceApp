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
        public NavigationBarViewModel NavigationBarViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            ContentViewModel.Dispose();
            NavigationBarViewModel.Dispose();
            base.Dispose();
        }
    }
}
