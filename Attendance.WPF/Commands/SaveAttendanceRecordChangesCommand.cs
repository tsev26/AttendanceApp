using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveAttendanceRecordChangesCommand : CommandBase
    {
        private readonly SelectedUserStore _selectedUserStore;
        private readonly INavigationService _closeModalNavigationService;

        public SaveAttendanceRecordChangesCommand(SelectedUserStore selectedUserStore, INavigationService closeModalNavigationService)
        {
            _selectedUserStore = selectedUserStore;
            _closeModalNavigationService = closeModalNavigationService;
        }

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
