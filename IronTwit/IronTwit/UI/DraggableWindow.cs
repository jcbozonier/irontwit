using System;
using System.Windows;
using System.Windows.Input;

namespace Unite.UI
{
    public class DraggableWindow : Window
    {
        private Point? _mouseDownPoint;
        private bool _dragging;

        protected Point _resizeMouseDownPoint;
        protected Size _mouseDownSize;
        protected bool _resizing;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Console.WriteLine("DraggableWindow.OnMouseLeftButtonDown");
            _mouseDownPoint = e.MouseDevice.GetPosition(null);

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _mouseDownPoint = null;

            if (_dragging)
            {
                _dragging = false;
                ReleaseMouseCapture();
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || _mouseDownPoint == null || _resizing)
            {
                base.OnMouseMove(e);
                return;
            }

            var currentPosition = e.MouseDevice.GetPosition(null);
            var moveVector = Point.Subtract(currentPosition, (Point)_mouseDownPoint);

            if (_dragging)
            {
                DragWindow(e, moveVector);
                return;
            }

            if (moveVector.Length >= 3) //this is a drag
            {
                _dragging = true;
                CaptureMouse();
                return;
            }

            base.OnMouseMove(e);
        }

        private void DragWindow(MouseEventArgs e, Vector dragVector)
        {
            var newPosition = PointToScreen(new Point(dragVector.X, dragVector.Y));

            Left = newPosition.X;
            Top = newPosition.Y;
        }
    }
}
