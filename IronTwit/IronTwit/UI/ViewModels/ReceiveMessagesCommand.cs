using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Net;

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

        public void Execute(object parameter, Action<WebException> webExceptionHandler)
        {
            if (_OnExecute != null)
            {
                try
                {
                    _OnExecute();
                }
                catch (WebException exception)
                {
                    if (webExceptionHandler != null)
                        webExceptionHandler(exception);
                    else
                        throw;
                }
            }
        }

        public void Execute(object parameter)
        {
            Execute(parameter, null);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
