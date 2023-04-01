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
    public class CreateUserCommand : CommandBase
    {
        private UserStore _userStore;
        private UserUpsertViewModel _userUpsertViewModel;
        private readonly CloseModalNavigationService _closeModalNavigationService;

        public CreateUserCommand(UserStore userStore, UserUpsertViewModel userUpsertViewModel, CloseModalNavigationService closeModalNavigationService)
        {
            _userStore = userStore;
            _userUpsertViewModel = userUpsertViewModel;
            _closeModalNavigationService = closeModalNavigationService;
        }

        public override void Execute(object? parameter)
        {
            User newUser = new User(_userUpsertViewModel.FirstName, _userUpsertViewModel.LastName, _userUpsertViewModel.Email, false, false);
            newUser.Group = _userUpsertViewModel.SelectedGroup;
            newUser.Keys.Add(new Key(_userUpsertViewModel.KeyValue));
            _userStore.AddUser(newUser);
            _closeModalNavigationService.Navigate();
        }
    }
}
