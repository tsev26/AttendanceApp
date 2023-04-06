using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;
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

        public MainViewModel(NavigationBarViewModel navigationBarViewModel,
                             NavigationStore navigationStore, 
                             ModalNavigationStore modalNavigationStore,
                             ActivityStore activityStore,
                             UserStore userStore,
                             GroupStore groupStore,
                             CurrentUser currentUser)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;

            NavigationBarViewModel = navigationBarViewModel;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModelViewModelChanged;



            ActivityProperty workActivityProperty = new ActivityProperty(false, true, false, true, false, new TimeSpan(15, 0, 0), "práce");
            Activity workActivity = new Activity("Práce", "P", workActivityProperty);

            activityStore.AddActivity(workActivity);

            ActivityProperty pauseActivityProperty = new ActivityProperty(false, true, true, false, false, new TimeSpan(0,0,0), "pauza");
            Activity pauseActivity = new Activity("Pauza", "A", pauseActivityProperty);

            activityStore.AddActivity(pauseActivity);

            ActivityProperty homeActivityProperty = new ActivityProperty(false, false, false, false, false, new TimeSpan(0, 0, 0), "domov");
            Activity homeActivity = new Activity("Domov", "D", homeActivityProperty);
            
            activityStore.AddActivity(homeActivity);

            ActivityProperty btActivityProperty = new ActivityProperty(true, true, false, true, true, new TimeSpan(10, 0, 0), "práce");
            Activity btActivity = new Activity("Služební cesta", "S", btActivityProperty);

            activityStore.AddActivity(btActivity);

            ActivityProperty vacationActivityProperty = new ActivityProperty(true, true, false, true, false, new TimeSpan(10, 0, 0), "dovolená");
            Activity vacationActivity = new Activity("Dovolená", "O", vacationActivityProperty);

            activityStore.AddActivity(vacationActivity);

            ActivityGlobalSetting activityGlobalSetting = new ActivityGlobalSetting(new TimeSpan(6, 0, 0), new TimeSpan(0, 30, 0), workActivity, homeActivity, new TimeSpan(8, 0, 0), new TimeSpan(4, 0, 0));
            activityStore.GlobalSetting = activityGlobalSetting;

            /*
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(workActivity);
            activityStore.AddActivity(pauseActivity);
            activityStore.AddActivity(pauseActivity);
            */


            Obligation obligation = new Obligation(8, true, new TimeOnly(9, 0, 0), new TimeOnly(15, 0, 0),  true, true, true, true, true, false, false, activityStore.Activities);
            Obligation obligationVedeni = new Obligation(6, true, new TimeOnly(12, 0, 0), new TimeOnly(13, 0, 0), true, true, false, true, true, false, false, activityStore.Activities);

            User admin = new User("admin", "admin", "tsevcu@gmail.com", true);
            admin.Keys.Add(new Key("admin"));
            admin.Keys.Add(new Key("aaa"));
            admin.Obligation = obligationVedeni;
            userStore.AddUser(admin);

            Group group = new Group("Základní", admin);
            group.Obligation = obligation;

            Group group2 = new Group("Vedení", admin);
            group2.Obligation = obligationVedeni;

            userStore.SetGroup(admin, group2);

            User user1 = new User("Tomáš", "Ševců", "tsevcu@gmail.com", false);
            user1.Keys.Add(new Key("tse"));
            user1.Keys.Add(new Key("tom"));
            user1.Keys.Add(new Key("tre"));
            //user1.Obligation = obligation;
            userStore.AddUser(user1);

            User user2 = new User("TEST", "8", "tsevcu@gmail.com", false);
            user2.Keys.Add(new Key("test"));
            userStore.AddUser(user2);

            User user3 = new User("Petr", "Ševců", "tsevcu@gmail.com", false);
            user3.Keys.Add(new Key("etr"));
            userStore.AddUser(user3);

            User user4 = new User("Eva", "Ševců", "tsevcu@gmail.com", false);
            user4.Keys.Add(new Key("eva"));
            userStore.AddUser(user4);


            userStore.SetGroup(user1, group);
            userStore.SetGroup(user2, group);
            userStore.SetGroup(user3, group);
            userStore.SetGroup(user4, group);


            groupStore.AddGroup(group);
            groupStore.AddGroup(group2);

            currentUser.LoadUser(user1);

            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 01, 8, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(homeActivity, new DateTime(2023, 3, 03, 15, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 04, 8, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(pauseActivity,new DateTime(2023, 3, 04, 14, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(homeActivity, new DateTime(2023, 3, 04, 15, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 07, 8, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(pauseActivity, new DateTime(2023, 3, 07, 14, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 07, 15, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(pauseActivity, new DateTime(2023, 3, 07, 15, 30, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 07, 15, 45, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(homeActivity, new DateTime(2023, 3, 15, 15, 0, 0));

            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 24, 8, 0, 0));
            currentUser.AttendanceRecordStore.AddAttendanceRecord(homeActivity, new DateTime(2023, 3, 24, 15, 0, 0));

            currentUser.AttendanceRecordStore.AddAttendanceRecord(workActivity, new DateTime(2023, 3, 26, 16, 0, 0));
            currentUser.User = null;
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
