using System.Collections.Generic;

namespace Unite.Messaging
{
    public class ServiceProvider
    {
        private List<IMessagingService> Services;

        public ServiceProvider()
        {
            Services = new List<IMessagingService>();
        }

        public void Add(params IMessagingService[] services)
        {
            Services.AddRange(services);
        }

        public IEnumerable<IMessagingService> GetServices()
        {
            return Services;
        }
    }
}
