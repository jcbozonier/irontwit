using System;
using System.Windows.Input;
using Unite.UI.Utilities;
using StructureMap;

namespace Unite.UI.ViewModels
{
    public class SendMessageCommand : ICommand
    {
        private Action _OnSendMessage;

        public SendMessageCommand(Action onSendMessage)
        {
            _OnSendMessage = onSendMessage;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if (_OnSendMessage != null)
                _OnSendMessage();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
