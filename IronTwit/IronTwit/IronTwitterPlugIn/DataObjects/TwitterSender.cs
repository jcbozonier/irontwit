using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniteMessaging;

namespace IronTwitterPlugIn.DataObjects
{
    public class TwitterSender : ISender
    {
        public SupportedServices Service
        {
            get { return SupportedServices.Twitter; }
        }

        public string AccountName
        {
            get; set;
        }
    }
}
