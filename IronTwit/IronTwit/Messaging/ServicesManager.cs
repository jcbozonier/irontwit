using System;
using System.Collections.Generic;

namespace Unite.Messaging
{
    public class ServicesManager : IMessagingService
    {
        private IServiceProvider Provider;

        public ServicesManager(IServiceProvider provider)
        {
            Provider = provider;
            Provider.CredentialsRequested += Provider_CredentialsRequested;
        }

        void Provider_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(sender, e); 
        }

        public bool CanAccept(Credentials credentials)
        {
            return true;
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

        public void SetCredentials(Credentials credentials)
        {
            var services = Provider.GetServices();
            foreach (var service in services)
            {
                if(service.CanAccept(credentials))
                    service.SetCredentials(credentials);
            }
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
    }
}
