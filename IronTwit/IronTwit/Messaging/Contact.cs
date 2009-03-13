using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public class Contact
    {
        public Guid ContactId
        { 
            get; set;
        }

        public IEnumerable<IIdentity> Identities
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }
}
