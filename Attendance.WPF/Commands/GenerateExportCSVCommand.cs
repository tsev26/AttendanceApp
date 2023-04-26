using Attendance.Domain.Models.Virtual;
using Attendance.EF;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class GenerateExportCSVCommand : CommandBase
    {
        private readonly AttendanceRecordStore _attendanceRecordStore;
        private readonly CurrentUserStore _currentUserStore;
        private readonly ExportViewModel _exportViewModel;

        public GenerateExportCSVCommand(AttendanceRecordStore attendanceRecordStore, CurrentUserStore currentUserStore, ExportViewModel exportViewModel)
        {
            _attendanceRecordStore = attendanceRecordStore;
            _currentUserStore = currentUserStore;
            _exportViewModel = exportViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter  is string value)
            {
                string fileNameType = "";
                if (value == "onlyMine")
                {
                    fileNameType = _currentUserStore.User.ToString();
                }
                string fileName = "export_" + fileNameType + "_" + _exportViewModel.Year + "-" + _exportViewModel.Month + ".csv";
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    CheckFileExists = false,
                    CheckPathExists = true,
                    FileName = fileName,
                    Filter = ".",
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Title = "Vyberte složku, kde se export vytvoří"
                };

                if (dialog.ShowDialog() == true)
                {
                    string dbPath = dialog.FileName;
                    List<UsersExportData> usersExportDatas = new List<UsersExportData>();
                    if (value == "onlyMine")
                    {
                        usersExportDatas = _attendanceRecordStore.LoadUsersExportData(_currentUserStore.User);
                    }
                    else if (value == "subordinate")
                    {
                        usersExportDatas = _attendanceRecordStore.LoadUsersExportData();
                    }
                }
            }
        }
    }
}
