using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class NullObjectMessageService : IMessagingService
    {
        public List<IMessage> GetMessages(string username, string password)
        {
            return new List<IMessage>();
        }

        public void SendMessage(string username, string password, string message, string recipient)
        {
           
        }
    }

    public class NullObjectMessage : IMessage
    {
        public string Text
        {
            get; set;
        }

        public ISender Sender
        {
            get; set;
        }
    }
}
