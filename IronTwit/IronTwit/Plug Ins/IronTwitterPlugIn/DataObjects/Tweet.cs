using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Unite.Messaging;

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

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// for de/serialization only (alias to Sender)
        /// </summary>
        public TwitterUser user
        {
            get { return (TwitterUser)Sender; }
            set { Sender = value; }
        }

        private TwitterUser _sender;
        public ISender Sender
        {
            get { return _sender; }
            set { _sender = (TwitterUser)value; }
        }
    }
}