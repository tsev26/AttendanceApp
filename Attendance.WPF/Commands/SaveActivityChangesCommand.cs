using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class SaveActivityChangesCommand : CommandBase
    {
        private readonly ActivityStore _activityStore;
        private readonly MessageStore _messageStore;
        private readonly ActivitiesViewModel _activitiesViewModel;

        public SaveActivityChangesCommand(ActivityStore activityStore, MessageStore messageStore, ActivitiesViewModel activitiesViewModel)
        {
            _activityStore = activityStore;
            _messageStore = messageStore;
            _activitiesViewModel = activitiesViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string type)
            {
                if (type == "activity")
                {
                    _messageStore.Message = "Aktivita " + _activitiesViewModel.SelectedActivity.Name + " upravena";
                    _activityStore.UpdateActivity(_activitiesViewModel.SelectedActivity);
                    
                }
                else if (type == "globalSetting")
                {
                    _messageStore.Message = "Globální nastavení aktualizováno";
                    _activityStore.UpdateActivityGlobalSetting(_activitiesViewModel.ActivityGlobalSetting);
                }
            }
            
        }
    }
}
