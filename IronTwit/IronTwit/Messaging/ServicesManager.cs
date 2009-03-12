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
            Provider.CredentialsRequested += Provider_CredentialsRequested;
        }

        void Provider_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(sender, e); 
        }

        public List<IMessage> GetMessages()
        {
            var messages = new List<IMessage>();
            var services = Provider.GetServices();

            foreach (var service in services)
            {
                messages.AddRange(service.GetMessages());
            }

            return messages;
        }

        public void SendMessage(string recipient, string message)
        {
            
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
    }
}
