using Bound.Net;
using IronTwit.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.ComponentModel;
using UniteMessaging;

namespace IronTwit.ViewModels
{
    public class MainView : INotifyPropertyChanged
    {
        /// <summary>
        /// Any user input the view model needs can be requested through
        /// this object. Instantiation is handled in IoC container.
        /// </summary>
        public IInteractionContext Interactions;

        /// <summary>
        /// The user's username for twitter.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The user's password for twitter. This is horrible way to hold this info imo.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// A list of all of the tweets that should be displayed.
        /// </summary>
        public ObservableCollection<IMessage> Messages { get; set; }
        /// <summary>
        /// A list of all of the user's messages.
        /// </summary>
        public ObservableCollection<IMessage> MyReplies { get; set; }

        private string _messageToSend;
        /// <summary>
        /// The message the user wants to send.
        /// </summary>
        public string MessageToSend
        {
            get
            {
                return _messageToSend;
            }
            set
            {
                _messageToSend = value;
                PropertyChanged.Notify(()=>MessageToSend);
            }
        }

        private string _Recipient;
        /// <summary>
        /// The recipient the user wants to send MessageToSend to.
        /// </summary>
        public string Recipient
        {
            get
            {
                return _Recipient;
            }
            set
            {
                _Recipient = value;
                PropertyChanged.Notify(()=>Recipient);
            }
        }

        /// <summary>
        /// Command object invoked by the InteractionContext (GUI) to send
        /// a message.
        /// </summary>
        public SendMessageCommand SendMessage { get; set; }
        /// <summary>
        /// Command object invoked by the InteractionContext (GUI) to
        /// receive a message.
        /// </summary>
        public ReceiveMessagesCommand ReceiveMessage { get; set; }
 
        public MainView(
            IInteractionContext interactionContext,
            IMessagingService utilities)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(utilities == null)
                throw new ArgumentNullException("utilities");

            Messages = new ObservableCollection<IMessage>();
            MyReplies = new ObservableCollection<IMessage>();

            Interactions = interactionContext;

            SendMessage = new SendMessageCommand(
                () =>
                    {
                        utilities.SendMessage(UserName, Password, MessageToSend, Recipient);
                        MessageToSend = "";
                        Recipient = "";
                    });

            ReceiveMessage = new ReceiveMessagesCommand(
                () =>
                    {
                        var result = utilities.GetMessages(UserName, Password);

                        Messages.Clear();

                        foreach (var message in result)
                        {
                            Messages.Add(message);
                        }
                    });

        }

        /// <summary>
        /// This must be called when the application first starts so
        /// that the model can go through the appropriate workflow
        /// to set up the UI for the user.
        /// </summary>
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

        /// <summary>
        /// Gets called whenever a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
