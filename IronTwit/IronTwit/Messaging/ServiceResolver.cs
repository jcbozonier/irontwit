using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class ServiceResolver
    {
        private static readonly Guid TwitterServiceId = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B565D7}");
        public Guid GetService(string address)
        {
            if (string.IsNullOrEmpty(address))
                return TwitterServiceId;
            if (address.Trim().Length == 0)
                return TwitterServiceId;
            if (address.StartsWith("@"))
                return TwitterServiceId;
            if (address.Contains("@"))
                throw new NotSupportedException("Email"); // Email
            throw new NotSupportedException("GChat");
        }
    }
}
