using System;
using System.Collections.Generic;
using System.Linq;

namespace Unite.Messaging
{
    public interface IServiceProvider
    {
        void Add(params IMessagingService[] services);
        IEnumerable<IMessagingService> GetServices();
        event EventHandler<CredentialEventArgs> CredentialsRequested;
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
    }
}
