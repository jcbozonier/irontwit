using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;

namespace Unite.Specs.Application_running
{
    [TestFixture]
    public class When_messages_are_refreshed : context
    {
        protected override void Context()
        {
            Model = ObjectFactory.GetInstance<MainView>();
            Model.Init();

            Model.Messages.Count.ShouldEqual(1);
        }

        protected override void Because()
        {
            Model.ReceiveMessage.Execute(null);
        }
        
        [Test]
        public void It_should_clear_previous_messages()
        {
            Model.Messages.Count.ShouldEqual(0);
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;


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


    public class TestTwitterUtilities : IMessagingService
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
                                 new Tweet() {Text = "testing", Recipient = new TwitterUser() {UserName = "darkxanthos"}}
                             }
                       : new List<IMessage>();
        }

        public void SendMessage(string recipient, string message)
        {
            Credentials = new Credentials() { UserName = "username", Password = "password" };
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
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<TestTwitterUtilities>();
            });

        }

    }
}
