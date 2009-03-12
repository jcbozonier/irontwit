using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessagingServiceRegistry
    {
        List<IMessagingService> RegisteredServices { get; }

        void Register(IMessagingService service);

        IMessagingService Get(Guid id);
    }
}
