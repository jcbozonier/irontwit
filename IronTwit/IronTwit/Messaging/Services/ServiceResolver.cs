using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class ServiceResolver : IServiceResolver
    {
        public ServiceResolver(IServiceProvider provider)
        {
            _Services = provider.GetServices();
        }

        private readonly IEnumerable<IMessagingService> _Services;

        public ServiceInformation GetService(string address)
        {
            foreach(var service in _Services)
            {
                if (service.CanFind(address))
                    return service.GetInformation();
            }

            return null;
        }
    }
}