using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using UniteMessaging;

namespace Specs.Application_running
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
        public string Username;
        public string Password;
        public string Message;
        public string Recipient;

        public int _Counter;

        public List<IMessage> GetMessages(string username, string password)
        {
            Username = username;
            Password = password;

            _Counter++;

            return _Counter == 1
                       ? new List<IMessage>()
                             {
                                 new Tweet() {Text = "testing", Sender = new TwitterUser() {AccountName = "darkxanthos"}}
                             }
                       : new List<IMessage>();
        }

        public void SendMessage(string username, string password, string message, string recipient)
        {
            Username = username;
            Password = password;
            Message = message;
            Recipient = recipient;
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
