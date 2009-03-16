using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public class MessagesReceivedEventArgs : EventArgs
    {
        public IEnumerable<IMessage> Messages
        {
            get; set;
        }
    }
}
