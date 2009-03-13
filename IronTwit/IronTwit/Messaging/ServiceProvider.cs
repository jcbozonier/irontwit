using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Unite.Messaging
{
    public interface IServiceProvider
    {
        void Add(params IMessagingService[] services);
        IEnumerable<IMessagingService> GetServices();
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        IMessagingService GetService(ServiceInformation info);
    }

    public class ServiceProvider : IServiceProvider
    {
        private readonly List<IMessagingService> Services;

        public ServiceProvider()
        {
            Services = new List<IMessagingService>();

            // We should probably try to discover the plug ins here...?
        }

        public void Add(params IMessagingService[] services)
        {
            foreach(var service in services)
            {
                service.CredentialsRequested += _GetCredentials;
                Services.Add(service);
            }
        }

        private void _GetCredentials(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(this, e);
        }

        public IEnumerable<IMessagingService> GetServices()
        {
            return Services;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public IMessagingService GetService(ServiceInformation info)
        {
            if(Services == null)
                throw new NullReferenceException("This service provider has a null collection of services. You should NEVER have this issue.");

            foreach (var service in Services)
            {
                if(service.GetInformation().Equals(info)) return service;
            }

            throw new InvalidOperationException("The requested service does not exist. You should NEVER see this.");
        }
    }
}
