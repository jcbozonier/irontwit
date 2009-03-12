using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServiceProvider
    {
        private IEnumerable<IMessagingService> Services;

        public ServiceProvider(params IMessagingService[] services)
        {
            Services = services;
        }

        public IEnumerable<IMessagingService> GetServices()
        {
            return Services;
        }
    }
}
