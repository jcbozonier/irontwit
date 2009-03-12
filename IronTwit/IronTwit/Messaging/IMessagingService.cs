using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessagingService
    {
        List<IMessage> GetMessages();
        void SendMessage(string recipient, string message);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
    }

    public class CredentialEventArgs : EventArgs, IServiceInformation
    {
        public string ServiceName { get; set; }
        public Guid ServiceID { get; set; }
    }

    public interface IServiceInformation
    {
        string ServiceName { get; }
        Guid ServiceID { get; }
    }
}
