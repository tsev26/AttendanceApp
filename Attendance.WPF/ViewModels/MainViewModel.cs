using Attendance.Domain.Models;
using Attendance.EF;
using Attendance.EF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;

namespace Attendance.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase CurrentModelViewModel => _modalNavigationStore.CurrentViewModel;

        public bool IsModalOpen => _modalNavigationStore.IsOpen;

        public MainViewModel(DbSQLiteContextFactory dbSQLiteContextFactory,
                             NavigationBarViewModel navigationBarViewModel,
                             NavigationStore navigationStore, 
                             ModalNavigationStore modalNavigationStore,
                             ActivityStore activityStore,
                             ActivityDataService activityDataService,
                             AttendanceRecordStore attendanceRecordStore,
                             UserStore userStore)
        {
            bool pathToDbExists = dbSQLiteContextFactory.InitDbSQLite();

            //if path to db file doesnt exist select new one where will be created or connected to existing
            if (!pathToDbExists)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    CheckFileExists = false,
                    CheckPathExists = true,
                    FileName = "attendanceDB.sqlite",
                    Filter = "Databázové soubory (*.sqlite)|*.sqlite|Složky|.",
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Title = "Vyberte soubor sqlite nebo složku kde se sqlite vytvoří"
                };

                if (dialog.ShowDialog() == true)
                {
                    string dbPath = dialog.FileName;
                    dbSQLiteContextFactory.SetDbPath(dbPath);
                }
            }
            bool add = dbSQLiteContextFactory.SetDefaultValuesIntoDb();

            activityStore.GlobalSetting = activityDataService.GetActivityGlobalSetting();

            if (add)
            {
                User admin = userStore.GetUserByKey("AAA").GetAwaiter().GetResult();
                List<Activity> activities = activityStore.LoadActivities().GetAwaiter().GetResult();
                Activity work = activities.First(a => a.Name == "Práce");
                Activity pause = activities.First(a => a.Name == "Pauza");
                Activity home = activities.First(a => a.Name == "Domov");

                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 3, 8, 0, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 3, 15, 0, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 4, 7, 58, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 4, 15, 20, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 5, 8, 0, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, pause, new DateTime(2023, 4, 5, 11, 48, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 5, 12, 0, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 5, 15, 12, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 6, 8, 05, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 6, 16, 02, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 7, 8, 01, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 7, 15, 08, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 10, 8, 09, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 10, 15, 35, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 11, 8, 12, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 11, 15, 48, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 12, 8, 02, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 12, 15, 33, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 13, 8, 22, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 13, 15, 1, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 14, 8, 2, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 14, 16, 50, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 17, 8, 45, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 17, 17, 01, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 18, 7, 54, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 18, 15, 30, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 19, 8, 20, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 19, 16, 13, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 20, 8, 10, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 20, 17, 20, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 21, 8, 11, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 21, 15, 47, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 24, 8, 02, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 24, 15, 45, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 25, 8, 0, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 25, 15, 37, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 26, 8, 27, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 26, 15, 15, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 27, 8, 8, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 27, 16, 8, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, work, new DateTime(2023, 4, 28, 8, 1, 0));
                attendanceRecordStore.AddAttendanceRecord(admin, home, new DateTime(2023, 4, 28, 15, 0, 0));

            }

            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;

            NavigationBarViewModel = navigationBarViewModel;

            

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModelViewModelChanged;
        }

        public NavigationBarViewModel NavigationBarViewModel { get; }

        private void OnCurrentModelViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModelViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged -= OnCurrentModelViewModelChanged;
            base.Dispose();
        }
    }
}
