using Attendance.Domain.Models;
using Attendance.WPF.Commands;
using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.WPF.ViewModels
{
    public class UsersCurrentActivityViewModel : ViewModelBase
    {
		private readonly AttendanceRecordStore _attendanceRecordStore;
        public UsersCurrentActivityViewModel(AttendanceRecordStore attendanceRecordStore,INavigationService closeModalNavigationService)
		{
			_attendanceRecordStore = attendanceRecordStore;
            CloseModalCommand = new CloseModalCommand(closeModalNavigationService);

			_attendanceRecordStore.UsersCurrentActivitiesLoad += AttendanceRecordStore_UsersCurrentActivitiesLoad;
			_attendanceRecordStore.LoadUsersCurrentActivities();
        }

		private void AttendanceRecordStore_UsersCurrentActivitiesLoad()
		{
            UsersCurrentActivities = _attendanceRecordStore.UsersCurrentActivities;
            OnPropertyChanged(nameof(UsersCurrentActivities));
        }

		public List<UsersCurrentActivity> UsersCurrentActivities { get; set; }

		public ICommand CloseModalCommand { get; }

        private string _searchUser;
        public string SearchUser
        {
			get
			{
				return _searchUser;
			}
			set
			{
				_searchUser = value;
				OnPropertyChanged(nameof(SearchUser));
                FilterUsers();
            }
		}

        private void FilterUsers()
        {
            if (SearchUser.Length < 3)
            {
                UsersCurrentActivities = _attendanceRecordStore.UsersCurrentActivities;
                OnPropertyChanged(nameof(UsersCurrentActivities));
                return;
            }

            var normalizedSearchUser = SearchUser.Normalize(NormalizationForm.FormD);
            var excludedCategories = new[] { UnicodeCategory.NonSpacingMark };
            var searchUserWithoutDiacritics = new string(normalizedSearchUser
                .Where(c => !excludedCategories.Contains(CharUnicodeInfo.GetUnicodeCategory(c)))
                .ToArray());

            var searchTerms = searchUserWithoutDiacritics.ToLower().Split(' ').ToList();
            var searchTermsWithoutDiacritics = searchTerms.Select(term =>
            {
                var normalizedTerm = term.Normalize(NormalizationForm.FormD);
                return new string(normalizedTerm
                    .Where(c => !excludedCategories.Contains(CharUnicodeInfo.GetUnicodeCategory(c)))
                    .ToArray());
            }).ToList();

            UsersCurrentActivities = _attendanceRecordStore.UsersCurrentActivities
                .Where(uca =>
                {
                    var normalizedFirstName = uca.User.FirstName.Normalize(NormalizationForm.FormD);
                    var firstNameWithoutDiacritics = new string(normalizedFirstName
                        .Where(c => !excludedCategories.Contains(CharUnicodeInfo.GetUnicodeCategory(c)))
                        .ToArray());

                    var normalizedLastName = uca.User.LastName.Normalize(NormalizationForm.FormD);
                    var lastNameWithoutDiacritics = new string(normalizedLastName
                        .Where(c => !excludedCategories.Contains(CharUnicodeInfo.GetUnicodeCategory(c)))
                        .ToArray());

                    return searchTermsWithoutDiacritics.All(term =>
                        firstNameWithoutDiacritics.ToLower().Contains(term) ||
                        lastNameWithoutDiacritics.ToLower().Contains(term));
                })
                .ToList();

            OnPropertyChanged(nameof(UsersCurrentActivities));
        }

		public override void Dispose()
		{
            _attendanceRecordStore.UsersCurrentActivitiesLoad -= AttendanceRecordStore_UsersCurrentActivitiesLoad;
            base.Dispose();
		}
	}
}
