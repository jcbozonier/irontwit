using Unite.Messaging;
using Unite.Messaging.Entities;

namespace IronTwitterPlugIn.DataObjects
{
    public class TwitterUser : IIdentity
    {
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