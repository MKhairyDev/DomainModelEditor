using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace DomainModelEditor.Behaviors
{
    public class Drag : Behavior<FrameworkElement>
    {
        private Point _elementStartPosition;
        private Point _mouseStartPosition;
        public double ContainerWidth
        {
            get { return (double)GetValue(ContainerWidthProperty); }
            set { SetValue(ContainerWidthProperty, value); }
        }
        public static readonly DependencyProperty ContainerWidthProperty = DependencyProperty.Register(
       "ContainerWidth", typeof(double), typeof(Behavior<FrameworkElement>), new PropertyMetadata(default(double)));

        public double ContainerHeight
        {
            get { return (double)GetValue(ContainerHeightProperty); }
            set { SetValue(ContainerHeightProperty, value); }
        }
        public static readonly DependencyProperty ContainerHeightProperty = DependencyProperty.Register(
       "ContainerHeight", typeof(double), typeof(Behavior<FrameworkElement>), new PropertyMetadata(default(double)));
        public double XAxis
        {
            get { return (double)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(
            "XAxis", typeof(double), typeof(Behavior<FrameworkElement>), new PropertyMetadata(default(double)));

        public double YAxis
        {
            get { return (double)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(
            "YAxis", typeof(double), typeof(Behavior<FrameworkElement>), new PropertyMetadata(default(double)));
        protected override void OnAttached()
        {
            Window parent = Application.Current.MainWindow;
            _elementStartPosition.X = XAxis;
            _elementStartPosition.Y = YAxis;
            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                _mouseStartPosition = e.GetPosition(parent);
                AssociatedObject.CaptureMouse();

            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();

                _elementStartPosition.X = XAxis;
                _elementStartPosition.Y = YAxis;

            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                var mousePos = e.GetPosition(parent);
                var diff = (mousePos - _mouseStartPosition);
                if (!AssociatedObject.IsMouseCaptured) return;

                XAxis = (double)(_elementStartPosition.X + diff.X);
                YAxis = (double)(_elementStartPosition.Y + diff.Y);

                if (XAxis < 0)
                    XAxis = 0;
                else if (XAxis + AssociatedObject.ActualWidth > ContainerWidth)
                    XAxis = ContainerWidth - AssociatedObject.ActualWidth;

                if (YAxis < 0)
                    YAxis = 0;
                else if (YAxis + AssociatedObject.ActualHeight > ContainerHeight)
                    YAxis = ContainerHeight - AssociatedObject.ActualHeight;

            };
        }
    }
}
