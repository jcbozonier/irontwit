using System;
using System.Collections.Generic;
using IronTwitterPlugIn.DataObjects;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using IIdentity=Unite.Messaging.IIdentity;
using IInteractionContext=Unite.Messaging.IInteractionContext;
using IMessagingServiceManager=Unite.Messaging.IMessagingServiceManager;

namespace Unite.Specs.Application_running
{
    public class TestTwitterUtilities : IMessagingServiceManager
    {
        public Guid ServiceId { get { return Guid.NewGuid(); } }
        public string ServiceName { get { return "TestTwitter"; } }

        public Credentials Credentials;
        public string Message;
        public string Recipient;

        public int _Counter;

        public bool CanAccept(Credentials credentials)
        {
            return true;
        }

        public List<IMessage> GetMessages()
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };

            _Counter++;

            return _Counter == 1
                       ? new List<IMessage>()
                             {
                                 new Tweet() {
                                     Text = "testing", 
                                     Address = new FakeUser() {UserName = "darkxanthos"},
                                    TimeStamp = DateTime.Now
                                 }
                             }
                       : new List<IMessage>();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };
            Message = message;
            Recipient = recipient.UserName;
        }

        public void SetCredentials(Credentials credentials)
        {
            Credentials = credentials;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public bool CanFind(string address)
        {
            return true;
        }

        public ServiceInformation GetInformation()
        {
            return new ServiceInformation();
        }

        public void StartReceiving()
        {
            if(MessagesReceived != null)
                MessagesReceived(this, new MessagesReceivedEventArgs(new []
                                                                         {
                                                                             new Message()
                                                                                 {
                                                                                     Text = "Message", 
                                                                                     Address = new Identity("yada yada", GetInformation()),
                                                                                     TimeStamp = DateTime.Now
                                                                                 }
                                                                         }));
        }

        public void StopReceiving()
        {
            
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public void SendMessage(string recipient, string message)
        {
            Message = message;
            Recipient = recipient;
        }
    }
    public class FakeUser : IIdentity
    {
        public string UserName { get; set; }
        public ServiceInformation ServiceInfo { get; set; }
    }
    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            return new Credentials()
            {
                UserName = "username",
                Password = "password",
                ServiceInformation = serviceInformation
            };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            return false;
        }
    }

    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {

            // Initialize the static ObjectFactory container

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<TestingInteractionContext>();
                x.ForRequestedType<IMessagingServiceManager>().TheDefaultIsConcreteType<TestTwitterUtilities>();
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
            });

        }

    }
}
