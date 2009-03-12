using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public struct MessagingAccount
    {
        public MessagingAccount(IMessagingService service, Credentials credentials)
        {
            Service = service;
            Credentials = credentials;
        }

        public IMessagingService Service;
        public Credentials Credentials;
    }
}
