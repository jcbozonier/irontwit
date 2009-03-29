using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace GoogleTalkPlugIn
{
    internal class GTalkMessage : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IIdentity Address
        {
            get; set;
        }
    }

    internal class GTalkUser : IIdentity
    {
        public string UserName
        {
            get; set;
        }

        public ServiceInformation ServiceInfo
        {
            get; set;
        }
    }
}
