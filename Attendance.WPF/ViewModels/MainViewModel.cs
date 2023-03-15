using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;

namespace Attendance.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase CurrentModelViewModel => _modalNavigationStore.CurrentViewModel;

        public bool IsModalOpen => _modalNavigationStore.IsOpen;

        public MainViewModel(NavigationStore navigationStore, 
                             ModalNavigationStore modalNavigationStore,
                             ActivityStore activityStore,
                             UserStore userStore)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModelViewModelChanged;

            ActivityProperty workActivityProperty = new ActivityProperty(false, false, true, TimeSpan.FromHours(6), TimeSpan.FromMinutes(30), false, false, false, false, "práce");
            Activity workActivity = new Activity("Práce", "P", workActivityProperty);

            activityStore.AddActivity(workActivity);

            User admin = new User("admin", "admin", "tsevcu@gmail.com", true);
            admin.Keys.Add(new Key("admin"));
            userStore.AddUser(admin);
        }

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
