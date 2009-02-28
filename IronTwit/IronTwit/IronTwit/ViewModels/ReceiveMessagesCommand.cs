using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Unite.UI.ViewModels
{
    public class ReceiveMessagesCommand : ICommand
    {
        private Action _OnExecute;

        public ReceiveMessagesCommand(Action onExecute)
        {
            _OnExecute = onExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_OnExecute != null)
                _OnExecute();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
