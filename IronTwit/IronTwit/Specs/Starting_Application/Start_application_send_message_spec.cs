using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Bound.Net;
using StructureMap.Pipeline;
using Unite.Messaging;

namespace Unite.Specs.Application_starting.Sending_messages
{
    [TestFixture]
    public class When_user_requests_to_send_message : context
    {
        [Test]
        public void It_should_be_sent()
        {
            Utilities.Message.ShouldNotBeNull();
        }

        [Test]
        public void It_should_update_the_UI_with_the_new_data()
        {
            UIUpdated.ShouldBeTrue();
        }

        [Test]
        public void It_should_use_the_correct_user_name()
        {
            Utilities.Credentials.UserName.ShouldNotBeNull();
        }

        [Test]
        public void It_should_use_the_correct_password()
        {
            Utilities.Credentials.Password.ShouldNotBeEmpty();
        }

        [Test]
        public void It_should_be_able_to_send_the_message()
        {
            Model.SendMessage.CanExecute(null).ShouldBeTrue();
        }

        [Test]
        public void It_should_match_the_one_provided_by_the_user()
        {
            Utilities.Message.ShouldEqual(MessageSent);
        }

        [Test]
        public void It_should_be_sent_to_the_correct_recipient()
        {
            Utilities.Recipient.ShouldEqual(Recipient);
        }

        [Test]
        public void It_should_clear_the_recipient_field()
        {
            Model.Recipient.ShouldEqual("");
        }

        [Test]
        public void It_should_clear_the_message_field()
        {
            Model.MessageToSend.ShouldEqual("");
        }

        protected override void Because()
        {
            Model.SendMessage.Execute(null);
        }

        private string MessageSent;
        private string Recipient;
        protected bool UIUpdated;

        protected override void Context()
        {
            Model.Init();
            Model.PropertyChanged += (s, e) => UIUpdated = true;
            Model.MessageToSend = MessageSent = "This is my message.";
            Model.Recipient = Recipient = "@testuser";
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;

        protected bool Message_was_sent
        {
            get; set;
        }
        

        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();

            Utilities = new TestTwitterUtilities();

            ObjectFactory.EjectAllInstancesOf<IMessagingService>();
            ObjectFactory.Inject<IMessagingServiceManager>(Utilities);

            Model = ObjectFactory.GetInstance<MainView>();
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestTwitterUtilities : IMessagingServiceManager
    {
        public Guid ServiceId { get { return Guid.NewGuid(); } }
        public string ServiceName { get { return "TestTwitter"; } }

        public Credentials Credentials;
        public string Message;
        public string Recipient;

        public bool CanAccept(Credentials credentials)
        {
            return true;
        }

        public List<IMessage> GetMessages()
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };

            return new List<IMessage>();
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
        public bool CanFind(string address)
        {
            throw new System.NotImplementedException();
        }

        public ServiceInformation GetInformation()
        {
            throw new System.NotImplementedException();
        }

        public void SendMessage(string recipient, string message)
        {
            Message = message;
            Recipient = recipient;
        }
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            throw new System.NotImplementedException();
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
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<TestTwitterUtilities>();
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
            });

        }

    }
}
