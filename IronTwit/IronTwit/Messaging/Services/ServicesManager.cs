using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class ServicesManager : IMessagingServiceManager
    {
        private readonly IServiceProvider Provider;
        private readonly IServiceResolver _Resolver;

        public ServicesManager(IServiceProvider provider)
        {
            Provider = provider;
            Provider.CredentialsRequested += Provider_CredentialsRequested;

            _Resolver = new ServiceResolver(Provider);
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

        public void SendMessage(IIdentity recipient, string message)
        {
            var service = Provider.GetService(recipient.ServiceInfo);
            service.SendMessage(recipient, message);
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

        public bool CanFind(string address)
        {
            var foundService = _Resolver.GetService(address);
            return foundService != null;
        }

        public ServiceInformation GetInformation()
        {
            return new ServiceInformation()
                       {
                           ServiceID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B56001}"),
                           ServiceName = "Service Manager"
                       };
        }

        public void SendMessage(string recipient, string message)
        {
            var serviceToUse = _Resolver.GetService(recipient);
            SendMessage(new Identity(recipient, serviceToUse), message);
        }
    }
}