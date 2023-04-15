using Attendance.Domain.Models;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Commands
{
    public class CreateGroupCommand : CommandBase
    {
        private GroupStore _groupStore;
        private GroupUpsertViewModel _groupUpsertViewModel;
        private CloseModalNavigationService _closeModalNavigationService;
        private CurrentUserStore _currentUser;

        public CreateGroupCommand(GroupStore groupStore, CurrentUserStore currentUser, GroupUpsertViewModel groupUpsertViewModel, CloseModalNavigationService closeModalNavigationService)
        {
            _groupStore = groupStore;
            _groupUpsertViewModel = groupUpsertViewModel;
            _closeModalNavigationService = closeModalNavigationService;
            _currentUser = currentUser;
        }

        public override void Execute(object? parameter)
        {
            Group newGroup = new Group(_groupUpsertViewModel.GroupName, _currentUser.User);
            _groupStore.AddGroup(newGroup);
            _closeModalNavigationService.Navigate("Skupina " + _groupUpsertViewModel.GroupName + " vytvořena");
        }
    }
}
