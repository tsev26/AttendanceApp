using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;

namespace Attendance.WPF.Clock
{
    public class Clock : Control
    {
        public static readonly DependencyProperty TimeProperty =
                DependencyProperty.Register("Time", typeof(DateTime), typeof(Clock), new PropertyMetadata(DateTime.Now, TimePropertyChanged));


        private static void TimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Clock)
            {
                Clock clock = d as Clock;
                clock.RaiseEvent(new RoutedPropertyChangedEventArgs<DateTime>((DateTime)e.OldValue, (DateTime)e.NewValue, TimeChangedEvent));
            }
        }

        public static DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(Clock), new PropertyMetadata(true));
        public static DependencyProperty ShowDateProperty = DependencyProperty.Register("ShowDate", typeof(bool), typeof(Clock), new PropertyMetadata(true));
        public static DependencyProperty ShowTimeProperty = DependencyProperty.Register("ShowTime", typeof(bool), typeof(Clock), new PropertyMetadata(true));
        public static RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(Clock));


        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, value); }
        }

        public bool ShowDate
        {
            get { return (bool)GetValue(ShowDateProperty); }
            set { SetValue(ShowDateProperty, value); }
        }

        public bool ShowTime
        {
            get { return (bool)GetValue(ShowTimeProperty); }
            set { SetValue(ShowTimeProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<DateTime> TimeChanged
        {
            add
            {
                AddHandler(TimeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TimeChangedEvent, value);
            }
        }

        public override void OnApplyTemplate()
        {
            OnTimeChanged(DateTime.Now);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            timer.Start();

            base.OnApplyTemplate();
        }

        protected virtual void OnTimeChanged(DateTime newTime)
        {
            UpdateTimeState(newTime);
            Time = newTime;
        }

        private void UpdateTimeState(DateTime time)
        {
            if (time.Hour > 6 && time.Hour < 18)
            {
                VisualStateManager.GoToState(this, "Day", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Night", false);
            }
        }
    }
}
