using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.Specs.Application_running
{
    [TestFixture]
    public class When_clicking_on_a_message : message_has_been_received
    {
        private string Sender;

        [Ignore]
        [Test]
        public void It_should_show_the_message_sender_in_the_recipient_field()
        {
            Model.Recipient.ShouldEqual(Model.SelectedMessage.Address.UserName);
        }

        protected override void Because()
        {
            Model.SelectedMessage = Model.Messages.First();
        }

        protected override void Context()
        {
            this.Model.ReceiveMessage.Execute(null);
        }
    }

    public abstract class message_has_been_received
    {
        protected MainView Model;
        protected UiMessage RecievedMessage;

        [TestFixtureSetUp]
        public void Setup()
        {
            var manager = new ServiceManager();

            ContainerBootstrapper.BootstrapStructureMap();
            Model = ObjectFactory.GetInstance<MainView>();
            ObjectFactory.EjectAllInstancesOf<IMessagingServiceManager>();
            ObjectFactory.Inject(typeof(IMessagingServiceManager), manager);

            Model.Init();

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class ServiceManager : IMessagingServiceManager
    {
        public bool CanAccept(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>()
                       {
                           new Message() {Text = "Testing", Address = new Identity("test", new ServiceInformation())}
                       };
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            throw new System.NotImplementedException();
        }

        public void SetCredentials(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public bool CanFind(string address)
        {
            throw new System.NotImplementedException();
        }

        public ServiceInformation GetInformation()
        {
            throw new System.NotImplementedException();
        }

        public void StartReceiving()
        {
            MessagesReceived(this, new MessagesReceivedEventArgs(GetMessages()));
        }

        public void StopReceiving()
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        public void SendMessage(string recipient, string message)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Message : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IIdentity Address
        {
            get; set;
        }
    }
}
