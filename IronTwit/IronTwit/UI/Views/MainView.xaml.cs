using System;
using System.Windows;
using System.Windows.Input;

namespace Unite.UI.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainView : DraggableWindow
    {
        public MainView(ViewModels.MainView viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            ((ViewModels.MainView)DataContext).Dispose();
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            ((ViewModels.IInitializeView)DataContext).Init();
        }

        private void Resizer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Resizer.CaptureMouse();
            _resizeMouseDownPoint = e.MouseDevice.GetPosition(null);
            _mouseDownSize = new Size(Width, Height);
            _resizing = true;
        }

        private void Resizer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _resizing = false;
            Resizer.ReleaseMouseCapture();
        }

        private void Resizer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizing)
                return;

            var currentPosition = e.MouseDevice.GetPosition(null);
            var moveVector = Point.Subtract(currentPosition, _resizeMouseDownPoint);

            if (_resizing)
            {
                ResizeWindow(moveVector);
                return;
            }
        }

        private void ResizeWindow(Vector dragVector)
        {
            var newWidth = _mouseDownSize.Width  + dragVector.X;
            var newHeight = _mouseDownSize.Height + dragVector.Y;
            Width = Math.Max(newWidth, 200);
            Height = Math.Max(newHeight, 300);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MessageToSend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage.Focus(); //need to cause MessageToSend to lose focus so binding will update viewmodel
                SendMessage.Command.Execute(null);
                OnMessageSent();
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            OnMessageSent();
        }

        private void OnMessageSent()
        {
            Recipient.Focus();
        }
    }
}
