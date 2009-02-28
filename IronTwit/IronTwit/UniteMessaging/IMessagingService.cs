using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniteMessaging
{
    public interface IMessagingService
    {
        List<IMessage> GetMessages(string username, string password);
        void SendMessage(string username, string password, string message, string recipient);
    }
}
