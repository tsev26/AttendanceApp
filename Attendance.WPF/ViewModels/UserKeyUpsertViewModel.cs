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
        private readonly SelectedDataStore _selectedUserStore;
        public UserKeyUpsertViewModel(SelectedDataStore selectedUserStore,
                                      MessageStore messageStore,
                                      INavigationService closeModalNavigationService)
        {
            _selectedUserStore = selectedUserStore;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
            KeyUpsertCommand = new KeyUpsertCommand(selectedUserStore, messageStore, closeModalNavigationService);
            NewKeyValue = selectedUserStore.SelectedKeyValue?.KeyValue;
        }

        public string Header => (_selectedUserStore.SelectedKeyValue?.KeyValue == null) ? "Přidání klíče" : "Úprava klíče";


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
                if (_selectedUserStore.SelectedKeyValue == null)
                {
                    _selectedUserStore.SelectedKeyValue = new Domain.Models.Key(NewKeyValue);
                }
                else
                {
                    _selectedUserStore.SelectedKeyValue.KeyValue = NewKeyValue;
                }
                OnPropertyChanged(nameof(NewKeyValue));
            }
        }

        public ICommand CloseModalCommand { get; }
        public ICommand KeyUpsertCommand { get; }

        public override void Dispose()
        {
            _selectedUserStore.SelectedKeyValue = null;
            base.Dispose();
        }
    }
}
