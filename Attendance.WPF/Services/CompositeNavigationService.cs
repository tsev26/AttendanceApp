using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Services
{
    public class CompositeNavigationService : INavigationService
    {
        private readonly IEnumerable<INavigationService> _navigationService;

        public CompositeNavigationService(params INavigationService[] navigationService)
        {
            _navigationService = navigationService;
        }

        public void Navigate(string message = "")
        {
            foreach (INavigationService navigationService in _navigationService)
            {
                navigationService.Navigate(message);
            }
        }
    }
}
