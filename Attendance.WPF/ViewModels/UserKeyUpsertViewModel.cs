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
    public class UserKeyUpsertViewModel : ViewModelBase
    {
        private readonly CurrentUser _currentUser;

        public UserKeyUpsertViewModel(CurrentUser currentUser,
                                      INavigationService closeModalNavigationService)
        {
            _currentUser = currentUser;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            KeyUpsertCommand = new KeyUpsertCommand(currentUser, closeModalNavigationService);
            NewKeyValue = _currentUser.SelectedKeyValue?.KeyValue;
        }

        public string Header => (_currentUser.SelectedKeyValue == null) ? "Přidání klíče" : "Úprava klíče";


        private string _newKeyValue;
        public string NewKeyValue
        {
            get
            {
                return _newKeyValue;
            }
            set
            {
                _newKeyValue = value;
                if (_currentUser.SelectedKeyValue == null)
                {
                    _currentUser.SelectedKeyValue = new Domain.Models.Key(NewKeyValue);
                }
                else
                {
                    _currentUser.SelectedKeyValue.KeyValue = NewKeyValue;
                }
                OnPropertyChanged(nameof(NewKeyValue));
            }
        }

        public ICommand CloseModalCommand { get; }
        public ICommand KeyUpsertCommand { get; }
    }
}
