using Attendance.Domain.Models.Virtual;
using Attendance.EF;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Formats.Asn1;
using System.Globalization;
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
        private readonly MessageStore _messageStore;

        public GenerateExportCSVCommand(AttendanceRecordStore attendanceRecordStore, CurrentUserStore currentUserStore, ExportViewModel exportViewModel, MessageStore messageStore)
        {
            _attendanceRecordStore = attendanceRecordStore;
            _currentUserStore = currentUserStore;
            _exportViewModel = exportViewModel;
            _messageStore = messageStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter  is string value)
            {
                int month = _exportViewModel.Month;
                int year = _exportViewModel.Year;
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
                    Filter = "",
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Title = "Vyberte složku, kde se export vytvoří"
                };

                if (dialog.ShowDialog() == true)
                {
                    string path = dialog.FileName;
                    List<UsersExportData> usersExportDatas = new List<UsersExportData>();
                    List<string> headerExportDatas = new List<string>();
                    headerExportDatas = _attendanceRecordStore.LoadHeaderExportData();
                    if (value == "onlyMine")
                    {
                        usersExportDatas = _attendanceRecordStore.LoadUsersExportData(month, year,_currentUserStore.User);
                    }
                    else if (value == "subordinate")
                    {
                        usersExportDatas = _attendanceRecordStore.LoadUsersExportData(month, year);
                    }

                    // Write the data to a CSV file
                    using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteField("Uživatel");
                        csv.WriteField("Datum");
                        foreach (var row in headerExportDatas)
                        {
                            csv.WriteField(row);
                            
                        }
                        csv.NextRecord();

                        foreach (var row in usersExportDatas)
                        {
                            csv.WriteField(row.UserName);
                            csv.WriteField(row.Date);
                            foreach (var header in headerExportDatas)
                            {
                                csv.WriteField(row.ActivityExportDatas.FirstOrDefault(a => a.ActivityName == header)?.Duration);
                            }
                            csv.NextRecord();
                        }
                    }

                    _messageStore.ModalMessage = "Export vygenerován";
                }
            }
        }
    }
}
