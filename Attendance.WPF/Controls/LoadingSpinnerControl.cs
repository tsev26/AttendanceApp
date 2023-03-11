using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Attendance.WPF.Controls
{
    public class LoadingSpinnerControl : Control
    {

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingSpinnerControl), new PropertyMetadata(false));
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }


        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter", typeof(double), typeof(LoadingSpinnerControl), new PropertyMetadata(100.0));
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }


        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(LoadingSpinnerControl), new PropertyMetadata(1.0));
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }


        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LoadingSpinnerControl), new PropertyMetadata(Brushes.Black));
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }


        static LoadingSpinnerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingSpinnerControl), new FrameworkPropertyMetadata(typeof(LoadingSpinnerControl)));
        }
    }
}
