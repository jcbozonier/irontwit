using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn;
using IronTwitterPlugIn.DataObjects;
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

        public Credentials GetCredentials(Guid serviceID, string serviceName)
        {
            return new Credentials()
            {
                UserName = "username",
                Password = "password",
                ServiceInformation = new ServiceInformation()
                {
                    ServiceID = serviceID,
                    ServiceName = serviceName
                }
            };
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

        public List<IMessage> GetMessages()
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };
            
            throw new WebException("Authentication failure.");
        }

        public void SendMessage(string recipient, string message)
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };
            Message = message;
            Recipient = recipient;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            throw new WebException("Authentication failure");
        }

        public Credentials GetCredentials(Guid serviceID, string serviceName)
        {
            throw new WebException("Authentication failure.");
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
