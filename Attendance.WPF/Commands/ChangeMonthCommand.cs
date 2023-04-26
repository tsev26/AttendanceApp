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
        private readonly ExportViewModel _exportViewModel;

        public ChangeMonthCommand(UserHistoryViewModel userHistoryViewModel)
        {
            _userHistoryViewModel = userHistoryViewModel;
        }

        public ChangeMonthCommand(ExportViewModel exportViewModel)
        {
            _exportViewModel = exportViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string shift)
            {
                if (shift == "addMonth")
                {
                    if (_userHistoryViewModel != null)
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
                        _userHistoryViewModel.SelectedIndex = -1;
                    }
                    else if (_exportViewModel != null)
                    {
                        if (_exportViewModel.Month == 12)
                        {
                            _exportViewModel.Year++;
                            _exportViewModel.Month = 1;
                        }
                        else
                        {
                            _exportViewModel.Month++;
                        }
                    }
                    
                }
                else if (shift == "substractMonth")
                {
                    if (_userHistoryViewModel != null)
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
                        _userHistoryViewModel.SelectedIndex = -1;
                    }
                    else if (_exportViewModel != null)
                    {
                        if (_exportViewModel.Month == 1)
                        {
                            _exportViewModel.Year--;
                            _exportViewModel.Month = 12;
                        }
                        else
                        {
                            _exportViewModel.Month--;
                        }
                    }
                }
                
            }
        }
    }
}
