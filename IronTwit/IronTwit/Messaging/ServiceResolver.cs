using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IServiceResolver
    {
        ServiceInformation GetService(string address);
    }

    public class ServiceResolver : IServiceResolver
    {
        public ServiceResolver(IServiceProvider provider)
        {
            _Services = provider.GetServices();
        }

        private IEnumerable<IMessagingService> _Services;

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
