using System;
using System.Collections.Generic;

namespace Unite.Messaging
{
    public interface IMessagingServiceRegistry
    {
        List<IMessagingService> RegisteredServices { get; }

        void Register(IMessagingService service);

        IMessagingService Get(Guid id);
    }
}
