using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using IronTwitterPlugIn;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Bound.Net;
using StructureMap.Pipeline;
using UniteMessaging;

namespace Specs.Application_starting.Sending_messages
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
            Utilities.Username.ShouldNotBeNull();
        }

        [Test]
        public void It_should_use_the_correct_password()
        {
            Utilities.Password.ShouldNotBeEmpty();
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

        protected bool Application_Asked_For_User_Name_And_Password
        {
            get
            {
                return !String.IsNullOrEmpty(Model.UserName) && !String.IsNullOrEmpty(Model.Password);
            }
        }

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
            ObjectFactory.Inject<IMessagingService>(Utilities);

            Model = ObjectFactory.GetInstance<MainView>();
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

        public List<IMessage> GetMessages(string username, string password)
        {
            Username = username;
            Password = password;

            return new List<IMessage>();
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
