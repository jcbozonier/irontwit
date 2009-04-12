using System;

namespace Unite.Messaging.Entities
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
            return AreEqual(this, obj);
        }

        public bool Equals(ServiceInformation obj)
        {
            return AreEqual(this, obj);
        }

        public static bool AreEqual(object a, object b)
        {
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return false;
            if (ReferenceEquals(a, b)) return true;
            if (!(a is IServiceInformation) || !(b is IServiceInformation)) return false;
            return AreEqual((IServiceInformation) a, (IServiceInformation) b);
        }

        public static bool AreEqual(IServiceInformation a, IServiceInformation b)
        {
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return false;
            if (ReferenceEquals(a, b)) return true;
            return Equals(a.ServiceName, b.ServiceName) && a.ServiceID.Equals(b.ServiceID);
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