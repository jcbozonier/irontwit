using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServicesManager : IMessagingService
    {
        public List<IMessage> GetMessages(Credentials credentials)
        {
            return new List<IMessage>();
        }

        public void SendMessage(Credentials credentials, string recipient, string message)
        {
            
        }
    }
}
