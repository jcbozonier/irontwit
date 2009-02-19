using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using IronTwit.Models;
using IronTwit.Models.Twitter;
using IronTwit.Utilities;
using IronTwit.ViewModels;
using NUnit.Framework;
using SpecUnit;
using StructureMap;

namespace Specs.Starting_Application
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
            Model.ApplicationStarting();
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
            ObjectFactory.EjectAllInstancesOf<ITwitterUtilities>();
            ObjectFactory.Inject<ITwitterUtilities>(Utilities);

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
            return new Credentials() { UserName = "testuser", Password = "testpassword" };
        }
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
            
            throw new WebException("Authentication failure.");

            return new List<Tweet>() { new Tweet() { text = "testing", user = new TwitterUser() { screen_name = "darkxanthos" } } };
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
            throw new WebException("Authentication failure.");
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
                x.ForRequestedType<ITwitterUtilities>().TheDefaultIsConcreteType<TestTwitterUtilities>();
            });

        }

    }

}
