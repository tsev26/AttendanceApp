using Attendance.Domain.Models;
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
    public class UserFixAttendanceRecordViewModel : ViewModelBase
    {
        private readonly SelectedDataStore _selectedUserStore;
        private readonly ActivityStore _activityStore;

        public UserFixAttendanceRecordViewModel(SelectedDataStore selectedUserStore, 
                                                ActivityStore activityStore,  
                                                AttendanceRecordStore attendanceRecordStore,
                                                INavigationService closeModalNavigationService, 
                                                INavigationService navigateFixesAttendance)
        {
            _selectedUserStore = selectedUserStore;
            _activityStore = activityStore;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            SaveChangeCommand = new SaveAttendanceRecordChangesCommand(selectedUserStore, attendanceRecordStore, this, closeModalNavigationService, navigateFixesAttendance);
            Header = selectedUserStore.AttendanceRecord == null ? "Přidání záznamu" : "Úprava záznamu";
            Activity = selectedUserStore.AttendanceRecord?.Activity;
            Date = selectedUserStore.AttendanceRecord?.Entry ?? DateTime.Now.Date;
            Hour = selectedUserStore.AttendanceRecord?.Entry.Hour ?? DateTime.Now.Hour;
            Minute = selectedUserStore.AttendanceRecord?.Entry.Minute ?? DateTime.Now.Minute;
        }

        public string Header { get; }

        public ICommand CloseModalCommand { get; }
        public ICommand SaveChangeCommand { get; }

        public List<Activity> Activities => _selectedUserStore.SelectedUser.UserObligation.AvailableActivities;

        public Activity Activity { get; set; }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                if (_date.Date > DateTime.Now.Date)
                {
                    _date = DateTime.Now.Date;
                }
                OnPropertyChanged(nameof(Date));
                Hours = (Date.Date == DateTime.Now.Date) ? Enumerable.Range(0, DateTime.Now.Hour + 1).ToList() : Enumerable.Range(0, 24).ToList();
                OnPropertyChanged(nameof(Hours));
                Minutes = (Date.Date == DateTime.Now.Date && Hour == DateTime.Now.Hour) ? Enumerable.Range(0, DateTime.Now.Minute + 1).ToList() : Enumerable.Range(0, 60).ToList();
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public List<int> Hours { get; set; }

        private int _hour;
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
                Minutes = (Date.Date == DateTime.Now.Date && Hour == DateTime.Now.Hour) ? Enumerable.Range(0, DateTime.Now.Minute + 1).ToList() : Enumerable.Range(0, 60).ToList();
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public List<int> Minutes {get; set; }

        private int _minute;
        public int Minute
        {
            get
            {
                return _minute;
            }
            set
            {
                _minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }

        public override void Dispose()
        {
            _selectedUserStore.AttendanceRecord = null;
            base.Dispose();
        }

    }
}
