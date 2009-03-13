using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unite.UI.Controls;

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
            Control.Content = "this is a link: http://www.cnn.com";
        }
    }

    public abstract class MessagesWithLinks_context
    {
        protected MessageTextView Control;

        [TestFixtureSetUp]
        public void Setup()
        {
            Control = new MessageTextView();
            
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
