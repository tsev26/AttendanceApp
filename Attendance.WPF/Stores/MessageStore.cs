using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Attendance.WPF.Stores
{
    public class MessageStore
    {
        private Timer _timer;

        public MessageStore()
        {
        }

        public event Action MessageChanged;

        public event Action ModalMessageChanged;

        public void Clear()
        {
            Message = "";
            ModalMessage = "";
        }

        private string _message;
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
                StartTimer();
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



        private void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            _timer = new Timer(5000);
            _timer.AutoReset = false;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Clear();
            _timer.Dispose();
        }

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
