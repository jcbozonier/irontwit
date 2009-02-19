using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using IronTwit.Utilities;
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
            ITwitterUtilities utilities)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(utilities == null)
                throw new ArgumentNullException("utilities");

            Tweets = new ObservableCollection<Tweet>();
            MyReplies = new ObservableCollection<Tweet>();

            Interactions = interactionContext;

            SendMessage = new SendMessageCommand(
                () =>
                    {
                        utilities.SendMessage(UserName, Password, MessageToSend, Recipient);
                    });

            ReceiveMessage = new ReceiveMessagesCommand(
                () =>
                    {
                        var result = utilities.GetUserMessages(UserName, Password);
                        foreach (var message in result)
                        {
                            Tweets.Add(message);
                        }
                    });

        }

        public void ApplicationStarting()
        {
            bool shouldRetryAuthorization = false;
            do
            {
                var credentials = Interactions.GetCredentials();
                UserName = credentials.UserName;
                Password = credentials.Password;

                try
                {
                    ReceiveMessage.Execute(null);
                }
                catch (WebException e)
                {
                    shouldRetryAuthorization = Interactions.AuthenticationFailedRetryQuery();
                }
            } while (shouldRetryAuthorization);
        }
    }
}
