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

        public string Text { get; set; }

        /// <summary>
        /// for de/serialization only (alias to Sender)
        /// </summary>
        public TwitterUser user
        {
            get { return (TwitterUser)Recipient; }
            set
            {
                if (!value.UserName.StartsWith("@"))
                    value.UserName = "@" + value.UserName;
                Recipient = value;
            }
        }

        public IIdentity Recipient { get; set; }
    }
}