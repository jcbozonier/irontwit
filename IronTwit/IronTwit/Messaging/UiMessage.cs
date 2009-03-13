using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class UiMessage : IMessage
    {
        public UiMessage(IMessage message, Contact contact)
        {
            Recipient = message.Recipient;
            Text = message.Text;
            Contact = contact;
        }

        public string Text
        {
            get; set;
        }

        public IIdentity Recipient
        {
            get; set;
        }

        public Contact Contact
        {
            get; set;
        }
    }
}
