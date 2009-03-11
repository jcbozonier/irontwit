using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Unite.Specs.ReceivingMessages
{
    [TestFixture]
    public class When_receiving_a_message_with_a_URI_in_it : MessagesWithLinks_context
    {
        [Test]
        public void It_should_hyperlink_the_URI_in_the_message()
        {
            
        }

        public override void Context()
        {
            
        }

        protected override void Because()
        {
            
        }
    }

    public abstract class MessagesWithLinks_context
    {

        [TestFixtureSetUp]
        public void Setup()
        {
            // Create fake messaging service that can return message text.
            // Have message text view consume the text and devise a way to inspect that
            // it worked.
            Context();
            Because();
        }

        public abstract void Context();

        protected abstract void Because();
    }
}
