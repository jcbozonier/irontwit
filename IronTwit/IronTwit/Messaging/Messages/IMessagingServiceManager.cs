using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Messages;

namespace Unite.Messaging
{
    public interface IMessagingServiceManager : IMessagingService
    {
        void SendMessage(string recipient, string message);
    }
}
