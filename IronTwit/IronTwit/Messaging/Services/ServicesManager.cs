using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class ServicesManager : IMessagingServiceManager
    {
        private readonly ServiceInformation _ServiceInfo = new ServiceInformation()
        {
            ServiceID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B56001}"),
            ServiceName = "Service Manager"
        };

        private readonly IServiceProvider _Provider;
        private readonly IServiceResolver _Resolver;

        private readonly IEnumerable<IMessagingService> _Services;

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public ServicesManager(IServiceProvider provider)
        {
            _Provider = provider;
            _Provider.CredentialsRequested += Provider_CredentialsRequested;
            _Provider.AuthorizationFailed += Provider_AuthorizationFailed;
            _Resolver = new ServiceResolver(_Provider);

            _Services = _Provider.GetServices();
        }

        void Provider_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            if (AuthorizationFailed != null)
                    AuthorizationFailed(sender, e);
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
            var services = _Services;

            foreach (var service in services)
            {
                messages.AddRange(service.GetMessages());
            }

            return messages;
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            var service = _Provider.GetService(recipient.ServiceInfo);
            service.SendMessage(recipient, message);
        }

        public void SetCredentials(Credentials credentials)
        {
            var services = _Services;

            foreach (var service in services)
            {
                if(service.CanAccept(credentials))
                    service.SetCredentials(credentials);
            }
        }

        public bool CanFind(string address)
        {
            var foundService = _Resolver.GetService(address);
            return foundService != null;
        }

        public ServiceInformation GetInformation()
        {
            return _ServiceInfo;
        }

        public void StartReceiving()
        {
            var services = _Services;

            foreach (var service in services)
            {
                service.MessagesReceived += service_MessagesReceived;
                service.StartReceiving();
            }
        }

        private void service_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            if (MessagesReceived != null)
                MessagesReceived(sender, e);
        }

        public void StopReceiving()
        {
            var services = _Services;

            foreach (var service in services)
            {
                service.MessagesReceived -= service_MessagesReceived;
                service.StopReceiving();
            }
        }

        public void SendMessage(string recipient, string message)
        {
            var serviceToUse = _Resolver.GetService(recipient);
            SendMessage(new Identity(recipient, serviceToUse), message);
        }
    }
}