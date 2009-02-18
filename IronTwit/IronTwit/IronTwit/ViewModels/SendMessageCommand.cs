using System;
using System.Windows.Input;
using IronTwit.Utilities;
using StructureMap;

namespace IronTwit.ViewModels
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
