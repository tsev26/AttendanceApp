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
    public class ExportViewModel : ViewModelBase
    {
        public ExportViewModel(AttendanceRecordStore attendanceRecordStore, CurrentUserStore currentUserStore, INavigationService closeModalNavigationService)
        {
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
            IsUserSuperVisor = currentUserStore.IsUserSuperVisor;
            ChangeMonthCommand = new ChangeMonthCommand(this);
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            GenerateExportCSVCommand = new GenerateExportCSVCommand(attendanceRecordStore, currentUserStore, this);
        }

        public ICommand ChangeMonthCommand { get; }
        public ICommand CloseModalCommand { get; }
        public ICommand GenerateExportCSVCommand { get; }

        public bool IsUserSuperVisor { get; }

        private int _year;
        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private int _month;
        

        public int Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
                OnPropertyChanged(nameof(Month));
                OnPropertyChanged(nameof(IsButtonNextMonthVisible));
            }
        }

        public bool IsButtonNextMonthVisible => !(Year == DateTime.Now.Year && Month == DateTime.Now.Month);
    }
}
