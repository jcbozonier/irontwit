using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServiceResolver
    {
        public ServiceResolver(IEnumerable<IMessagingService> services)
        {
            _Services = services;
        }

        private static readonly Guid TwitterServiceId = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B565D7}");
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
