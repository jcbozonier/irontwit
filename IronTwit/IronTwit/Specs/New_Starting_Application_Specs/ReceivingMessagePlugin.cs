using System;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.New_Starting_Application_Specs
{
    public class ReceivingMessagePlugin : FakePlugin
    {
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public override void StartReceiving()
        {
            CredentialsRequested(this, new CredentialEventArgs(){ServiceInfo = ServiceInformation});
            base.StartReceiving();
        }
    }
}
