using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging
{
    public interface IMessagingServiceManager : IMessagingService
    {
        void SendMessage(string recipient, string message);
    }

    public interface IMessagingService
    {
        bool CanAccept(Credentials credentials);
        List<IMessage> GetMessages();
        void SendMessage(IIdentity recipient, string message);
        void SetCredentials(Credentials credentials);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        bool CanFind(string address);
        ServiceInformation GetInformation();
    }

    public class CredentialEventArgs : EventArgs
    {
        public ServiceInformation ServiceInfo;
    }

    public interface IServiceInformation
    {
        string ServiceName { get; }
        Guid ServiceID { get; }
    }
}
