using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Stores
{
    public class MessageStore
    {
        private string _message;

        public MessageStore()
        {
        }

        public void Clear()
        {
            Message = "";
            ModalMessage = "";
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnMessageChanged();
            }
        }

        private string _modalMessage;
        public string ModalMessage
        {
            get
            {
                return _modalMessage;
            }
            set
            {
                _modalMessage = value;
                OnModalMessageChanged();
            }
        }

        public bool HasMessage => Message.Length > 0;

        public event Action MessageChanged;

        public event Action ModalMessageChanged;

        private void OnModalMessageChanged()
        {
            ModalMessageChanged?.Invoke();
        }

        private void OnMessageChanged()
        {
            MessageChanged?.Invoke();
        }
    }
}
