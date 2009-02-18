using System;
using System.Collections.Generic;
using System.Windows.Input;
using IronTwit.Models.Twitter;
using IronTwit.Utilities;

namespace IronTwit.ViewModels
{
    public class ReceiveMessagesCommand : ICommand
    {
        private ITwitterUtilities _Utilities;
        public string Username;
        public string Password;

        public ReceiveMessagesCommand(ITwitterUtilities utilities)
        {
            _Utilities = utilities;
        }

        public event EventHandler CanExecuteChanged;

        public Action<List<Tweet>> CommandExecuted;

        public void Execute(object parameter)
        {
            var result = _Utilities.GetUserMessages(Username, Password);
            if(CommandExecuted == null) throw new NullReferenceException("Executes was not set.");
            CommandExecuted(result);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
