using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecUnit;
using Unite.Messaging;

namespace Unite.Specs.Using_Services
{
    [TestFixture]
    public class When_getting_messages : context
    {
        [Test]
        public void It_should_get_messages_from_all_available_services()
        {
            Messages.Count().ShouldEqual(2);
        }

        protected override void Because()
        {
            Messages = ServiceManager.GetMessages();
        }

        protected override void Context()
        {
            
        }
    }

    public abstract class context
    {
        protected IMessagingService ServiceManager;
        protected ServiceProvider ServiceProvider;
        protected IEnumerable<IMessage> Messages;
        protected Credentials MyCredentials;

        [TestFixtureSetUp]
        public void Setup()
        {
            MyCredentials = new Credentials() { UserName = "username", Password = "password" };
            ServiceProvider = new ServiceProvider();
            ServiceProvider.Add(new FauxMessageService("test 1"), new FauxMessageService("test2"));
            ServiceManager = new ServicesManager(ServiceProvider);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class FauxMessageService : IMessagingService
    {
        private string MessageText;

        public FauxMessageService(string messageText)
        {
            MessageText = messageText;
        }

        public bool CanAccept(Credentials credentials)
        {
            return true;
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>()
                       {
                           new Message()
                               {
                                   Recipient = new Recipient(){UserName = "darkxanthos"},
                                   Text = MessageText
                               }
                       };
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            throw new System.NotImplementedException();
        }

        public void SetCredentials(Credentials credentials)
        {
            
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public bool CanFind(string address)
        {
            throw new System.NotImplementedException();
        }

        public ServiceInformation GetInformation()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Recipient : IIdentity
    {
        public string UserName
        {
            get; set;
        }

        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }

    public class Message : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IIdentity Recipient
        {
            get; set;
        }
    }
}
