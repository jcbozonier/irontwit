using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    //Service specific (i.e. not a "Contact" which is a semantic grouping of IIdentities under one friendly name)
    //Service specific "contacts" are called Identities because that is the actual identity for the system
    //General identities are call contacts because they represent a single person to contact.
    public interface IIdentity
    {
        /// <summary>
        /// For Twitter, e.g. '@darkxanthos'
        /// For Email, e.g. 'darkxanthos@gmail.com'
        /// </summary>
        string UserName { get; }
        ServiceInformation ServiceInfo { get; }
    }

    public class Identity : IIdentity
    {
        public Identity(string userName, ServiceInformation serviceInformation)
        {
            UserName = userName;
            ServiceInfo = serviceInformation;
        }

        public string UserName { get; private set; }
        public ServiceInformation ServiceInfo { get; private set; }
    }
}
