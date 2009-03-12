using System;
using Unite.Messaging;
using Unite.UI.Utilities;
using IronTwitterPlugIn;
using NUnit.Framework;
using SpecUnit;
using System.Collections.Generic;

namespace Unite.Specs.Application_running.sending_messages
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

        [Test]
        public void It_should_not_remove_any_letters_from_the_message()
        {
            DataAccess.SentMessages.Reverse();
            DataAccess.SentMessages[0].EndsWith("it.").ShouldBeTrue();
        }

        protected string Recipient = "@ChadBoyer" ;

        protected string Message =
            "I need to test to ensure that my twitter client is breaking up messages only at word boundaries. This will come in handy not just for making long tweets read better when split but also for displaying URLs. My code can't handle a single long word right now though (think a word larger than 140 chars :). Once that becomes a *real* problem, I'll look into fixing it.";

        protected override void Because()
        {
            Utilities.SendMessage(
                Recipient,
                Message);
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

    public class TestSender : IRecipient
    {
        public Guid ServiceId { get { return Guid.NewGuid(); } }
        public string UserName { get; set; }
    }

    public class TestTwitterDataAccess : ITwitterDataAccess
    {
        public int MessagesSent;
        public List<string> SentMessages;

        public TestTwitterDataAccess()
        {
            SentMessages = new List<string>();
        }

        public string SendMessage(Credentials credentials, string message)
        {
            MessagesSent++;
            SentMessages.Add(message);
            return "result message";
        }

        public string GetMessages(Credentials credentials)
        {
            return "result message";
        }
    }
}
