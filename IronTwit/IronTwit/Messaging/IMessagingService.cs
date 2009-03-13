using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessagingService
    {
        bool CanAccept(Credentials credentials);
        List<IMessage> GetMessages();
        void SendMessage(string recipient, string message);
        void SetCredentials(Credentials credentials);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
    }

    public class CredentialEventArgs : EventArgs, IServiceInformation
    {
        public string ServiceName { get; set; }
        public Guid ServiceID { get; set; }
        public override bool Equals(object obj)
        {
            return ServiceInformation.AreEqual(this, obj);
        }
    }

    public interface IServiceInformation
    {
        string ServiceName { get; }
        Guid ServiceID { get; }
    }
}
