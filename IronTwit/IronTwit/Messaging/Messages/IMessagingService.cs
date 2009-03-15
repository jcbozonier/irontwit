using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Messages
{
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
}