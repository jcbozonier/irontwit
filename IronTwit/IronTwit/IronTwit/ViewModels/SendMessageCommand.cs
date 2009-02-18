using System;
using System.Windows.Input;
using IronTwit.Utilities;
using StructureMap;

namespace IronTwit.ViewModels
{
    public class SendMessageCommand : ICommand
    {
        private Utilities.ITwitterUtilities _Utilities;
        public string Username;
        public string Password;

        public SendMessageCommand(ITwitterUtilities utilities)
        {
            _Utilities = utilities;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if(parameter == null) 
                throw new ArgumentNullException("parameter");
            if(!(parameter is string))
                throw new ArgumentException("parameter was of the wrong type.");

            _Utilities.SendMessage(Username, Password, (string)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
