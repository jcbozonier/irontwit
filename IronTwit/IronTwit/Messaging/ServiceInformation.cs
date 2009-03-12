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
    }
}
