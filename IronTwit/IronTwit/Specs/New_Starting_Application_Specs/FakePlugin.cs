using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.New_Starting_Application_Specs
{
    public class FakePlugin : IMessagingService
    {
        public static ServiceInformation ServiceInformation = 
            new ServiceInformation(){ServiceID = Guid.NewGuid(), ServiceName = "Fake"};

        public static IMessagingService Fake;

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public bool CanAccept(Credentials credentials)
        {
            return Fake.CanAccept(credentials);
        }

        public List<IMessage> GetMessages()
        {
            return Fake.GetMessages();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            Fake.SendMessage(recipient, message);
        }

        public void SetCredentials(Credentials credentials)
        {
            Fake.SetCredentials(credentials);
        }

        public bool CanFind(string address)
        {
            return Fake.CanFind(address);
        }

        public ServiceInformation GetInformation()
        {
            return Fake.GetInformation();
        }

        public virtual void StartReceiving()
        {
            Fake.StartReceiving();
        }

        public virtual void StopReceiving()
        {
            Fake.StopReceiving();
        }
    }
}
