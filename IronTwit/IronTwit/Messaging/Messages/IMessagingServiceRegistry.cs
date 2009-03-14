using System;
using System.Collections.Generic;

namespace Unite.Messaging.Messages
{
    public interface IMessagingServiceRegistry
    {
        List<IMessagingService> RegisteredServices { get; }

        void Register(IMessagingService service);

        IMessagingService Get(Guid id);
    }
}