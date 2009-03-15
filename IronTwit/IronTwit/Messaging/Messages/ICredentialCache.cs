using System;

namespace Unite.Messaging.Messages
{
    public interface ICredentialCache
    {
        void Add(Guid serviceId, Credentials credentials);
        void Clear(Guid serviceId, string userName);
        bool Contains(Guid serviceId, string userName);
        void Clear(Guid serviceId);
        void ClearAll();
    }
}