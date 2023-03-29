using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class ActivitiesViewModel : ViewModelBase
    {
        private readonly ActivityStore _activityStore;

        public ActivitiesViewModel(ActivityStore activityStore, Services.INavigationService navigationActivityCreateService)
        {
            _activityStore = activityStore;
            NavigateAddActivityCommand = new NavigateCommand(navigationActivityCreateService);
            SaveActivityChangesCommand = new SaveActivityChangesCommand(activityStore, this);
            ActivityViewShowsCommand = new ActivityViewShowsCommand(this);

            ActivityGlobalSetting = _activityStore.GlobalSetting.Clone();

            _activityStore.ActivitiesChange += ActivityStore_ActivitiesChange;
            _activityStore.GlobalSettingChange += ActivityStore_GlobalSettingChange;
            ActivityStore_ActivitiesChange();
        }

        private void ActivityStore_GlobalSettingChange()
        {
            OnPropertyChanged(nameof(ActivityGlobalSetting));
        }

        private void ActivityStore_ActivitiesChange()
        {
            Activities = _activityStore.Activities.Select(a => a.Clone()).ToList();
            OnPropertyChanged(nameof(Activities));
        }

        public ICommand NavigateAddActivityCommand { get; }
		public ICommand SaveActivityChangesCommand { get; }
        public ICommand ActivityViewShowsCommand { get; }


        private int _selectedActivityIndex = -1;
		public int SelectedActivityIndex
        {
			get
			{
				return _selectedActivityIndex;
			}
			set
			{
				_selectedActivityIndex = value;
				OnPropertyChanged(nameof(SelectedActivityIndex));
                OnPropertyChanged(nameof(IsActivitySelected));
                OnPropertyChanged(nameof(IsNoActivitySelected));
                OnPropertyChanged(nameof(SelectedActivity));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Shortcut));
                OnPropertyChanged(nameof(IsPlan));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(IsPause));
                OnPropertyChanged(nameof(HasPause));
                OnPropertyChanged(nameof(GroupByName));
            }
		}

        public bool IsActivitySelected => SelectedActivityIndex != -1;

        public bool IsNoActivitySelected => !IsActivitySelected;

        public ActivityGlobalSetting ActivityGlobalSetting { get; set; }

        public List<Activity> Activities { get; set; }

        public Activity? SelectedActivity => (IsActivitySelected) ? Activities[SelectedActivityIndex] : null;

        public string Name => (IsActivitySelected) ? Activities[SelectedActivityIndex].Name : "";
        public string Shortcut => (IsActivitySelected) ? Activities[SelectedActivityIndex].Shortcut : "";

        public bool IsPlan => (IsActivitySelected) ? Activities[SelectedActivityIndex].Property.IsPlan : false;
        public bool Count => (IsActivitySelected) ? Activities[SelectedActivityIndex].Property.Count : false;
        public bool IsPause => (IsActivitySelected) ? Activities[SelectedActivityIndex].Property.IsPause : false;
        public bool HasPause => (IsActivitySelected) ? Activities[SelectedActivityIndex].Property.HasPause : false;
        public string GroupByName => (IsActivitySelected) ? Activities[SelectedActivityIndex].Property.GroupByName : "";

        public override void Dispose()
        {
            _activityStore.ActivitiesChange -= ActivityStore_ActivitiesChange;
            _activityStore.GlobalSettingChange -= ActivityStore_GlobalSettingChange;
            base.Dispose();
        }
    }
}
