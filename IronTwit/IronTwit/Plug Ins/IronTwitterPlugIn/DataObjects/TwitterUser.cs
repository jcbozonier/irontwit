using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace IronTwitterPlugIn.DataObjects
{
    public class TwitterUser : IIdentity
    {
        /// <summary>
        /// I don't want anyone outside this library creating these.
        /// </summary>
        internal TwitterUser()
        {
            
        }

        /// <summary>
        /// for de/serialization only (alias to UserName)
        /// </summary>
        public string screen_name
        {
            get { return UserName; }
            set { UserName = value; }
        }

        public string UserName { get; set; }
        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }
}