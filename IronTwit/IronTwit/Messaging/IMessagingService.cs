using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessagingService
    {
        Guid ServiceId { get; }
        string ServiceName { get; }
        List<IMessage> GetMessages(Credentials credentials);
        void SendMessage(Credentials credentials, string recipient, string message);
    }
}
