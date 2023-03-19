using Attendance.Domain.Models;
using Attendance.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        public UserProfileViewModel(CurrentUser currentUser)
        {
            UpdatesOnUser = new User(currentUser.User.FirstName, currentUser.User.LastName, currentUser.User.Email, currentUser.User.IsAdmin, true);

            UpdatesOnUser.Keys = currentUser.User.Keys;


            if (currentUser.User.Obligation == null)
            {
                UpdatesOnUser.Obligation = new Obligation(currentUser.User.Group.Obligation);
                ObligationFromUser = false;
            }
            else
            {
                UpdatesOnUser.Obligation = new Obligation(currentUser.User.Obligation);
                ObligationFromUser = true;
            }
            if (currentUser.User.Group != null)
            {
                UpdatesOnUser.Group = new Group(currentUser.User.Group);
            }
            
        }

        public User UpdatesOnUser { get; set; }

        public bool ObligationFromUser { get; }

        public string ObligationFrom => "(nastavení " + (ObligationFromUser ? "z uživatele" : "ze skupiny") + ")";

        public override void Dispose()
        {
            UpdatesOnUser = null;
            base.Dispose();
        }
    }
}
