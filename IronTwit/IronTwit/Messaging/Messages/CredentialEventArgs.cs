using System;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Messages
{
    public class CredentialEventArgs : EventArgs
    {
        public ServiceInformation ServiceInfo;

        public bool Equals(CredentialEventArgs obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ServiceInfo, ServiceInfo);
        }

        public override int GetHashCode()
        {
            return (ServiceInfo != null ? ServiceInfo.GetHashCode() : 0);
        }
    }
}