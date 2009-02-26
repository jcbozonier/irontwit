using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronTwit.Models.Twitter
{
    public class TwitterUser : ISender
    {
        public SupportedServices Service { get { return SupportedServices.Twitter; } }

        /// <summary>
        /// for de/serialization only (alias to AccountName)
        /// </summary>
        public string screen_name
        {
            get { return AccountName; }
            set { AccountName = value; }
        }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }
    }
}