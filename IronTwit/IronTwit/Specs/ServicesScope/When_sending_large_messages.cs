using IronTwit.Utilities;
using NUnit.Framework;
using SpecUnit;
using System.Collections.Generic;

namespace Specs.Application_running.sending_messages
{
    [TestFixture]
    public class When_sending_a_large_message_with_no_recipient : context
    {
        [Test]
        public void It_should_be_broken_up_into_multiple_messages()
        {
            DataAccess.MessagesSent.ShouldBeGreaterThan(1);
        }

        [Test]
        public void It_includes_the_recipient_in_each_message()
        {
            DataAccess.SentMessages.ForEach(message => message.Contains(Recipient).ShouldBeTrue());
        }

        [Test]
        public void It_clips_each_message_to_be_at_or_below_the_max_message_length()
        {
            DataAccess.SentMessages.ForEach(message => message.Length.ShouldBeLessThan(MaxMessageLength+1));
        }

        protected string Recipient = "@ChadBoyer";

        protected override void Because()
        {
            Utilities.SendMessage(
                "username", 
                "password", 
                "This is a test message from my new twitter client. If everything goes as planned, this message should be split up into multiple messages with each one directed at you. Sorry for the long message but I wanted to make sure it got split up at least once.  :) This is a test message from my new twitter client. If everything goes as planned, this message should be split up into multiple messages with each one directed at you. Sorry for the long message but I wanted to make sure it got split up at least once.  :)", 
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
