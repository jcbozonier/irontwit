using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;

namespace Unite.Messaging.Messages
{
    public interface IMessagingService
    {
        bool CanAccept(Credentials credentials);
        List<IMessage> GetMessages();
        void SendMessage(IIdentity recipient, string message);
        void SetCredentials(Credentials credentials);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
        bool CanFind(string address);
        ServiceInformation GetInformation();
        void StartReceiving();
        void StopReceiving();
        event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}