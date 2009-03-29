using System;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace IronTwitterPlugIn.DataObjects
{
    public class Tweet : IMessage
    {
        /// <summary>
        /// for de/serialization only (alias to Text)
        /// </summary>
        public string text
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Text { get; set; }

        /// <summary>
        /// for de/serialization only (alias to Sender)
        /// </summary>
        public TwitterUser user
        {
            get { return (TwitterUser)Address; }
            set
            {
                if (!value.UserName.StartsWith("@"))
                    value.UserName = "@" + value.UserName;
                Address = value;
            }
        }
        public int id { get; set; }

        public IIdentity Address { get; set; }
        public DateTime TimeStamp
        {
            get; set;
        }
    }
}