using Attendance.WPF.Stores;

namespace Attendance.WPF.Services
{
    public class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly MessageStore _messageStore;
        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore, MessageStore messageStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _messageStore = messageStore;
        }

        public void Navigate(string message = "")
        {
            _messageStore.Message = message;
            _messageStore.ModalMessage = "";
            _modalNavigationStore.Close();
        }
    }
}
