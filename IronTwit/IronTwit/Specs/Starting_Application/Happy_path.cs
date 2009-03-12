using System;
using System.Collections.Generic;
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
        

        protected bool Application_Asked_For_User_Name_And_Password
        {
            get
            {
                return !String.IsNullOrEmpty(Model.UserName) && !String.IsNullOrEmpty(Model.Password);
            }
        }

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
            ObjectFactory.Inject<IMessagingService>(Utilities);
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
            Credentials = new Credentials()
            {
                UserName = "testuser",
                Password = "testpassword",
                ServiceInformation = new ServiceInformation()
                {
                    ServiceID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B565D7}"),
                    ServiceName = "test"
                }
            };
        }

        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            WasUserAuthenticated = true;
            return Credentials;
        }

        public bool AuthenticationFailedRetryQuery()
        {
            IsUserNotifiedOfAuthenticationFailure = true;
            return false;
        }
    }

    public class TestTwitterUtilities : IMessagingService
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

            return new List<IMessage>(){new Tweet(){Text="testing",Recipient=new TwitterUser(){UserName = "darkxanthos"}}};
        }

        public void SendMessage(string recipient, string message)
        {
            Message = message;
            Recipient = recipient;
        }

        public void SetCredentials(Credentials credentials)
        {
            Credentials = credentials;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
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
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<TestTwitterUtilities>();
            });

        }

    }
}
