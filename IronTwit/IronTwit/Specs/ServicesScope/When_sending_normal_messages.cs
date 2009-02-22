using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Utilities;
using NUnit.Framework;
using SpecUnit;

namespace Specs.ServicesScope.Sending_normal_messages
{

    [TestFixture]
    public class When_sending_a_large_message_with_no_recipient : context
    {
        [Test]
        public void It_should_not_be_broken_up_into_multiple_messages()
        {
            DataAccess.MessagesSent.ShouldEqual(1);
        }

        [Test]
        public void It_clips_each_message_to_be_at_or_below_the_max_message_length()
        {
            DataAccess.SentMessages.ForEach(message => message.Length.ShouldBeLessThan(MaxMessageLength + 1));
        }

        protected string Recipient=null;

        protected override void Because()
        {
            Utilities.SendMessage(
                "username",
                "password",
                "This is a test message from",
                Recipient);
        }

        protected override void Context()
        {
        }
    }

    [TestFixture]
    public class When_sending_a_large_message_with_recipient : context
    {
        [Test]
        public void It_should_not_be_broken_up_into_multiple_messages()
        {
            DataAccess.MessagesSent.ShouldEqual(1);
        }

        [Test]
        public void It_includes_the_recipient_in_the_message()
        {
            DataAccess.SentMessages.ForEach(message => message.Contains(Recipient).ShouldBeTrue());
        }

        [Test]
        public void It_clips_each_message_to_be_at_or_below_the_max_message_length()
        {
            DataAccess.SentMessages.ForEach(message => message.Length.ShouldBeLessThan(MaxMessageLength + 1));
        }

        protected string Recipient = "@ChadBoyer";

        protected override void Because()
        {
            Utilities.SendMessage(
                "username",
                "password",
                "This is a test message from",
                Recipient);
        }

        protected override void Context()
        {
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected TwitterUtilities Utilities;
        protected TestTwitterDataAccess DataAccess;
        protected readonly int MaxMessageLength = 140;

        [TestFixtureSetUp]
        public void Setup()
        {
            DataAccess = new TestTwitterDataAccess();
            Utilities = new TwitterUtilities(DataAccess, MaxMessageLength);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestTwitterDataAccess : ITwitterDataAccess
    {
        public int MessagesSent;
        public List<string> SentMessages;

        public TestTwitterDataAccess()
        {
            SentMessages = new List<string>();
        }

        public string SendMessage(string username, string password, string message)
        {
            MessagesSent++;
            SentMessages.Add(message);
            return "result message";
        }

        public string GetFriendsTimelineAsJSON(string username, string password)
        {
            return "result message";
        }
    }
}
