using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;

namespace Unite.Messaging
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public IServiceInformation ServiceInformation { get; set; }
    }
}
