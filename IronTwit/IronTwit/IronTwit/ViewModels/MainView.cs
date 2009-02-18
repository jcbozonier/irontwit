using System;
using System.Collections.ObjectModel;
using System.Windows;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using StructureMap;

namespace IronTwit.ViewModels
{
    public class MainView : DependencyObject
    {
        public IInteractionContext Interactions;

        public string UserName { get; set; }
        public string Password { get; set; }

        public ObservableCollection<Tweet> Tweets { get; set; }
        public ObservableCollection<Tweet> MyReplies { get; set; }

        public string MessageToSend { get; set; }
        public string Recipient { get; set; }
        public SendMessageCommand SendMessage { get; set; }

        public MainView(IInteractionContext interactionContext, SendMessageCommand command)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(command == null)
                throw new ArgumentNullException("command");

            Interactions = interactionContext;
            
            SendMessage = command;
        }

        public void ApplicationStarting()
        {
            var credentials = Interactions.GetCredentials();
            UserName = credentials.UserName;
            Password = credentials.Password;

            SendMessage.Username = UserName;
            SendMessage.Password = Password;
        }
    }
}
