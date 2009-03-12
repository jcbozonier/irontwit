using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace IronTwitterPlugIn.DataObjects
{
    public class TwitterUser : ISender
    {
        public Guid ServiceId { get { return TwitterUtilities.SERVICE_ID; } }

        /// <summary>
        /// for de/serialization only (alias to UserName)
        /// </summary>
        public string screen_name
        {
            get { return UserName; }
            set { UserName = value; }
        }

        public string UserName { get; set; }
    }
}