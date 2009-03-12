using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServicesManager : IMessagingService
    {
        private ServiceProvider Provider;

        public ServicesManager(ServiceProvider provider)
        {
            Provider = provider;
        }

        public List<IMessage> GetMessages(Credentials credentials)
        {
            var messages = new List<IMessage>();
            var services = Provider.GetServices();

            foreach (var service in services)
            {
                messages.AddRange(service.GetMessages(credentials));
            }

            return messages;
        }

        public void SendMessage(Credentials credentials, string recipient, string message)
        {
            
        }
    }
}
