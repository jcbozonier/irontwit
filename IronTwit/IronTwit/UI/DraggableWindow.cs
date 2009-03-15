using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Unite.UI
{
    public class DraggableWindow : Window
    {
        private Point _dragOffset;
        private bool _dragging;

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            _dragging = true;

            _dragOffset = e.MouseDevice.GetPosition(null);

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            _dragging = false;
            this.ReleaseMouseCapture();

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (!_dragging)
                return;

            var currentOffsetPosition = e.MouseDevice.GetPosition(null);
            var dragVector = Point.Subtract(currentOffsetPosition, _dragOffset);
            var newPosition = PointToScreen(new Point(dragVector.X, dragVector.Y));

            Left = newPosition.X;
            Top = newPosition.Y;

            base.OnMouseMove(e);
        }
    }
}
