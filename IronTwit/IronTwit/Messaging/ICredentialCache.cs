using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
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
