using System;

namespace Unite.Messaging.Entities
{
    public interface IServiceInformation
    {
        string ServiceName { get; }
        Guid ServiceID { get; }
    }
}
