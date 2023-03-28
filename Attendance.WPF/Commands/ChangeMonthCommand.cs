using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class ChangeMonthCommand : CommandBase
    {
        private readonly UserHistoryViewModel _userHistoryViewModel;

        public ChangeMonthCommand(UserHistoryViewModel userHistoryViewModel)
        {
            _userHistoryViewModel = userHistoryViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string shift)
            {
                if (shift == "addMonth")
                {
                    if (_userHistoryViewModel.Month == 12)
                    {
                        _userHistoryViewModel.Year++;
                        _userHistoryViewModel.Month = 1;
                    }
                    else
                    {
                        _userHistoryViewModel.Month++;
                    }
                    
                }
                else if (shift == "substractMonth")
                {
                    if (_userHistoryViewModel.Month == 1)
                    {
                        _userHistoryViewModel.Year--;
                        _userHistoryViewModel.Month = 12;
                    }
                    else
                    {
                        _userHistoryViewModel.Month--;
                    }
                }
                _userHistoryViewModel.SelectedIndex = -1;
            }
        }
    }
}
