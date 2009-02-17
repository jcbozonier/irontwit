using System;
using System.Windows.Input;

namespace IronTwit.ViewModels
{
    public class SendMessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            throw new System.NotImplementedException();
        }

        public bool CanExecute(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
