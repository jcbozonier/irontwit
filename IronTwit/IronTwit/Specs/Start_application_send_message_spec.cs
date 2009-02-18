using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using StructureMap.Pipeline;

namespace Specs.Sending_messages
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
            Utilities.Message.ShouldEqual(Model.MessageToSend);
        }

        [Test]
        public void It_should_be_sent_to_the_correct_recipient()
        {
            Utilities.Recipient.ShouldEqual(Model.Recipient);
        }

        protected override void Because()
        {
            Model.SendMessage.Execute(null);
        }

        protected override void Context()
        {
            Model.ApplicationStarting();
            Model.MessageToSend = "This is my message.";
            Model.Recipient = "@testuser";
        }
    }

    [TestFixture]
    public class When_main_view_is_shown_for_the_first_time : context
    {
        [Test]
        public void It_should_ask_for_user_name_and_password()
        {
            Application_Asked_For_User_Name_And_Password.ShouldBeTrue();
        }

        protected override void Because()
        {
            Model.ApplicationStarting();
        }

        protected override void Context()
        {
            //throw new System.NotImplementedException();
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

            ObjectFactory.EjectAllInstancesOf<ITwitterUtilities>();
            ObjectFactory.Inject<ITwitterUtilities>(Utilities);

            Model = ObjectFactory.GetInstance<MainView>();
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestTwitterUtilities : ITwitterUtilities
    {
        public string Username;
        public string Password;
        public string Message;
        public string Recipient;

        public List<Tweet> GetUserMessages(string username, string password)
        {
            Username = username;
            Password = password;

            return null;
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
    }

    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {

            // Initialize the static ObjectFactory container

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<TestingInteractionContext>();
                x.ForRequestedType<ITwitterUtilities>().TheDefaultIsConcreteType<TestTwitterUtilities>();
            });

        }

    }
}
