using Bound.Net;
using Unite.UI.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.ComponentModel;
using Unite.Messaging;

namespace Unite.UI.ViewModels
{
    public interface IInitializeView
    {
        void Init();
    }

    public class MainView : IInitializeView, INotifyPropertyChanged
    {
        /// <summary>
        /// Any user input the view model needs can be requested through
        /// this object. Instantiation is handled in IoC container.
        /// </summary>
        public IInteractionContext Interactions;

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

        private IMessagingServiceManager _MessagingService;

        public MainView(
            IInteractionContext interactionContext,
            IMessagingServiceManager messagingService)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(messagingService == null)
                throw new ArgumentNullException("messagingService");

            _MessagingService = messagingService;
            _MessagingService.CredentialsRequested += messagingService_CredentialsRequested;

            Messages = new ObservableCollection<IMessage>();
            MyReplies = new ObservableCollection<IMessage>();

            Interactions = interactionContext;

            SendMessage = new SendMessageCommand(
                () =>
                    {
                        _MessagingService.SendMessage(Recipient, MessageToSend);
                        Recipient = "";
                        MessageToSend = "";
                    });

            ReceiveMessage = new ReceiveMessagesCommand(
                () =>
                    {
                        var result = _MessagingService.GetMessages();
                        Messages.Clear();

                        foreach (var message in result)
                        {
                            Messages.Add(message);
                        }
                    });

        }

        void messagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            var credentials = Interactions.GetCredentials(e.ServiceInfo);
            _MessagingService.SetCredentials(credentials);
        }

        /// <summary>
        /// This must be called when the application first starts so
        /// that the model can go through the appropriate workflow
        /// to set up the UI for the user.
        /// </summary>
        public void Init()
        {
            bool shouldRetryAuthorization = false;
            do
            {
                ReceiveMessage.Execute(
                    null, 
                    webException => 
                        shouldRetryAuthorization = Interactions.AuthenticationFailedRetryQuery());

            } while (shouldRetryAuthorization);
        }

        /// <summary>
        /// Gets called whenever a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
