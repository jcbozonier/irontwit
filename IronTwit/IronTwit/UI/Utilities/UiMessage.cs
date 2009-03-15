using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace Unite.UI.Utilities
{
    public class UiMessage : IMessage
    {
        public UiMessage(IMessage message, Contact contact)
        {
            Address = message.Address;
            Text = message.Text;
            Contact = contact;
        }

        public string Text
        {
            get; set;
        }

        public IIdentity Address
        {
            get; set;
        }

        public Contact Contact
        {
            get; set;
        }
    }
}