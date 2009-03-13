using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServiceInformation : IServiceInformation
    {
        public string ServiceName
        {
            get; set;
        }

        public Guid ServiceID
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ServiceInformation)) return false;
            return Equals((ServiceInformation) obj);
        }

        public bool Equals(ServiceInformation obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ServiceName, ServiceName) && obj.ServiceID.Equals(ServiceID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ServiceName != null ? ServiceID.GetHashCode() : 0) * 397) ^ ServiceID.GetHashCode();
            }
        }
    }
}
