using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using StructureMap;
using StructureMap.Pipeline;

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
        public ReceiveMessagesCommand ReceiveMessage { get; set; }
 
        public MainView(
            IInteractionContext interactionContext, 
            SendMessageCommand sendMessageCommand,
            ReceiveMessagesCommand receiveMessagesCommand)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(sendMessageCommand == null)
                throw new ArgumentNullException("sendMessageCommand");

            Tweets = new ObservableCollection<Tweet>();
            MyReplies = new ObservableCollection<Tweet>();

            Interactions = interactionContext;

            SendMessage = sendMessageCommand;
            ReceiveMessage = receiveMessagesCommand;
            ReceiveMessage.CommandExecuted = (result) =>
                                                 {
                                                     foreach (var message in result)
                                                     {
                                                         Tweets.Add(message);
                                                     }
                                                 };

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
