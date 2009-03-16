using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;

namespace Unite.Specs.Application_starting
{
    
    [TestFixture]
    public class When_main_view_is_shown_for_the_first_time : context
    {

        [Test]
        public void It_should_get_messages_for_user()
        {
            Model.Messages.ShouldNotBeEmpty();
        }

        [Test]
        public void It_should_use_credentials_provided_by_user_to_get_messages()
        {
            Utilities.Credentials.ShouldEqual(InteractionContext.Credentials);
        }

        protected override void Because()
        {
            Model.Init();
        }

        protected override void Context()
        {
            Model = ObjectFactory.GetInstance<MainView>();
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;
        protected FakeInteractionContext InteractionContext;


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
            InteractionContext = new FakeInteractionContext();

            ObjectFactory.EjectAllInstancesOf<IMessagingService>();
            ObjectFactory.EjectAllInstancesOf<IInteractionContext>();
            ObjectFactory.Inject<IMessagingServiceManager>(Utilities);
            ObjectFactory.Inject<IInteractionContext>(InteractionContext);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class FakeInteractionContext : IInteractionContext
    {
        public bool IsUserNotifiedOfAuthenticationFailure;
        public bool WasUserAuthenticated;
        public Credentials Credentials;

        public FakeInteractionContext()
        {
            
        }

        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            WasUserAuthenticated = true;
            Credentials = new Credentials()
            {
                UserName = "testuser",
                Password = "testpassword",
                ServiceInformation = serviceInformation
            };
            return Credentials;
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
            return
                (credentials.ServiceInformation.Equals(new ServiceInformation()
                                                           {ServiceID = this.ServiceId, ServiceName = this.ServiceName}));
        }

        public List<IMessage> GetMessages()
        {
            CredentialsRequested(this, new CredentialEventArgs());

            return new List<IMessage>(){new Tweet(){Text="testing",Address=new FakeUser(){UserName = "darkxanthos"}}};
        }

        public void SendMessage(IIdentity recipient, string message)
        {
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
            return true;
        }

        public ServiceInformation GetInformation()
        {
            return new ServiceInformation()
                       {
                           ServiceID = ServiceId,
                           ServiceName = ServiceName
                       };
        }

        public void StartReceiving()
        {
            throw new System.NotImplementedException();
        }

        public void StopReceiving()
        {
            throw new System.NotImplementedException();
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
        public string UserName
        {
            get; set;
        }

        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials()
        {
            return new Credentials()
            {
                UserName = "username",
                Password = "password"
            };
        }

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
                x.ForRequestedType<IMessagingServiceManager>().TheDefaultIsConcreteType<TestTwitterUtilities>();
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
            });

        }

    }
}
