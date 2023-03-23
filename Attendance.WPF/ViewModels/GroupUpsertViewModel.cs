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
    public class GroupUpsertViewModel : ViewModelBase
    {

        public GroupUpsertViewModel(GroupStore groupStore, CurrentUser currentUser,CloseModalNavigationService closeModalNavigationService)
        {
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            CreateGroupCommand = new CreateGroupCommand(groupStore, currentUser, this, closeModalNavigationService);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand CreateGroupCommand { get; }

        private string _groupName;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

    }
}
