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
        private readonly SelectedUserStore _selectedUserStore;
        private readonly ActivityStore _activityStore;

        public UserFixAttendanceRecordViewModel(SelectedUserStore selectedUserStore, ActivityStore activityStore,  INavigationService closeModalNavigationService)
        {
            _selectedUserStore = selectedUserStore;
            _activityStore = activityStore;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            SaveChangeCommand = new SaveAttendanceRecordChangesCommand(selectedUserStore, closeModalNavigationService);
            Header = selectedUserStore.AttendanceRecord == null ? "Přidání záznamu" : "Úprava záznamu";
            Activity = selectedUserStore.AttendanceRecord?.Activity;
            Date = selectedUserStore.AttendanceRecord?.Entry ?? DateTime.Now;
            Hour = selectedUserStore.AttendanceRecord?.Entry.Hour ?? DateTime.Now.Hour;
            Minute = selectedUserStore.AttendanceRecord?.Entry.Minute ?? DateTime.Now.Minute;
        }

        public string Header { get; }

        public ICommand CloseModalCommand { get; }
        public ICommand SaveChangeCommand { get; }

        public List<Activity> Activities => _activityStore.Activities;

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
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(Hours));
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public List<int> Hours => (Date == DateTime.Now.Date) ? Enumerable.Range(DateTime.Now.Hour, 23 - DateTime.Now.Hour).ToList() : Enumerable.Range(0, 23).ToList();

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
                OnPropertyChanged(nameof(Hours));
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public List<int> Minutes => (Date == DateTime.Now.Date && Hour == DateTime.Now.Hour) ? Enumerable.Range(DateTime.Now.Minute, 59 - DateTime.Now.Minute).ToList() : Enumerable.Range(0, 59).ToList();

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


    }
}
