using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwitterPlugIn;
using NUnit.Framework;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Messages;

namespace Unite.Specs.TwitterServicesScope
{
    [TestFixture]
    public class When_receiving_messages : receiving_context
    {
        [Test]
        public void It_should_use_the_credentials_provided_by_the_user()
        {
            DataAccess.ProvidedCredentials.ShouldEqual(UserCredentials);
        }

        [Test]
        public void It_should_request_credentials()
        {
            CredentialsRequested.ShouldBeTrue();
        }

        [Test]
        public void It_should_get_all_messages()
        {
            ReceivedMessages.Count.ShouldEqual(1);
        }

        protected override void Because()
        {
            ReceivedMessages = Utilities.GetMessages();
        }

        protected override void Context()
        {
            
        }
    }

    [TestFixture]
    public abstract class receiving_context
    {
        protected TwitterUtilities Utilities;
        protected TestRxTwitterDataAccess DataAccess;
        protected readonly int MaxMessageLength = 140;
        protected bool CredentialsRequested;
        protected List<IMessage> ReceivedMessages;
        protected Credentials UserCredentials;

        [TestFixtureSetUp]
        public void Setup()
        {
            DataAccess = new TestRxTwitterDataAccess();
            Utilities = new TwitterUtilities(DataAccess, MaxMessageLength);
            Utilities.CredentialsRequested += Utilities_CredentialsRequested;

            Context();
            Because();
        }

        void Utilities_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            CredentialsRequested = true;
            UserCredentials = new Credentials()
                                  {
                                      UserName = "darkxanthos",
                                      Password = "password",
                                      ServiceInformation = e.ServiceInfo
                                  };
            Utilities.SetCredentials(UserCredentials);
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestRxTwitterDataAccess : ITwitterDataAccess
    {
        public int MessagesSent;
        public List<string> SentMessages;
        public Credentials ProvidedCredentials;

        public TestRxTwitterDataAccess()
        {
            SentMessages = new List<string>();
        }

        public string SendMessage(Credentials credentials, string message)
        {
            ProvidedCredentials = credentials;
            MessagesSent++;
            SentMessages.Add(message);
            return "result message";
        }

        public string GetMessages(Credentials credentials)
        {
            ProvidedCredentials = credentials;
            return "[{text:'test message'}]";
        }
    }
}
