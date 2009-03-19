using System;
using System.Collections.Generic;
using System.Net;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;

namespace Unite.Specs.Starting_Application
{
    [TestFixture]
    public class When_wrong_credentials_are_provided_to_the_app : context
    {
        [Test]
        public void It_should_tell_the_user_and_ask_if_the_user_wants_to_try_again()
        {
            InteractionContext.IsUserNotifiedOfAuthenticationFailure.ShouldBeTrue();
        }

        protected override void Because()
        {
            Model.Init();
        }

        protected FakeInteractionContext InteractionContext;

        protected override void Context()
        {
            InteractionContext = new FakeInteractionContext();

            ObjectFactory.EjectAllInstancesOf<IInteractionContext>();
            ObjectFactory.Inject<IInteractionContext>(InteractionContext);

            Model = ObjectFactory.GetInstance<MainView>();
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;

        protected bool Message_was_sent
        {
            get;
            set;
        }


        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();

            Utilities = new TestTwitterUtilities();
            ObjectFactory.EjectAllInstancesOf<IMessagingService>();
            ObjectFactory.Inject<IMessagingService>(Utilities);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class FakeInteractionContext : IInteractionContext
    {
        public bool IsUserNotifiedOfAuthenticationFailure;

        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            throw new System.NotImplementedException();
        }

        public bool AuthenticationFailedRetryQuery()
        {
            IsUserNotifiedOfAuthenticationFailure = true;
            return false;
        }
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
            var firstCredentials = new Credentials() { UserName = "username", Password = "password" };
            Credentials = firstCredentials;
            
            AuthorizationFailed(this, new CredentialEventArgs(){ServiceInfo = new ServiceInformation(){ServiceID = ServiceId, ServiceName = ServiceName}});

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
            Recipient = recipient;
            Message = message;
        }
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            throw new WebException("Authentication failure");
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
