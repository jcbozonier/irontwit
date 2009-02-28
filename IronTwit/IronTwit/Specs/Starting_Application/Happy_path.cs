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

namespace Unite.Specs.Application_starting
{
    
    [TestFixture]
    public class When_main_view_is_shown_for_the_first_time : context
    {
        [Test]
        public void It_should_ask_for_user_name_and_password()
        {
            Application_Asked_For_User_Name_And_Password.ShouldBeTrue();
        }

        [Test]
        public void It_should_get_messages_for_user()
        {
            Model.Messages.ShouldNotBeEmpty();
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

        public bool AuthenticationFailedRetryQuery()
        {
            IsUserNotifiedOfAuthenticationFailure = true;
            return false;
        }

        public Credentials GetCredentials()
        {
            return new Credentials() {UserName = "testuser", Password = "testpassword"};
        }
    }

    public class TestTwitterUtilities : IMessagingService
    {
        public string Username;
        public string Password;
        public string Message;
        public string Recipient;

        public List<IMessage> GetMessages(string username, string password)
        {
            Username = username;
            Password = password;

            return new List<IMessage>(){new Tweet(){Text="testing",Sender=new TwitterUser(){AccountName = "darkxanthos"}}};
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
