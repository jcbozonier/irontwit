using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jabber.client;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace GoogleTalkPlugIn
{
    public class GoogleTalkMessagingService : IMessagingService
    {
        private JabberClient _Client;
        private Credentials _Credentials;

        private static readonly ServiceInformation _ServiceInformation = new ServiceInformation()
                                                             {
                                                                 ServiceID = Guid.NewGuid(),
                                                                 ServiceName = "GoogleTalk"
                                                             };

        public bool CanAccept(Credentials credentials)
        {
            return !credentials.UserName.Contains("@");
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            CredentialsRequested(this, new CredentialEventArgs() { ServiceInfo = _ServiceInformation });
            if(_Credentials == null)
                throw new Exception("Your credentials can not still be null. This should NEVER happen.");

            _Client = new JabberClient();

            var client = _Client;

            client.AutoPresence = false;
            client.AutoRoster = false;
            client.AutoReconnect = -1;

            client.User = _Credentials.UserName;
            client.Password = _Credentials.Password;
            client.Server = "gmail.com";
            client.Port = 5222;
            client.Resource = "Unit3";

            client.OnError += (s,e) =>
                                  {
                                      throw new Exception(e.Message);
                                  };


            client.OnAuthError += (s, e) =>
                                      {
                                          if(AuthorizationFailed != null)
                                              AuthorizationFailed(this, new CredentialEventArgs(){ServiceInfo = _ServiceInformation});
                                      };

            client.OnAuthenticate += s =>
                                         {
                                             client.Message(recipient.UserName, message);
                                             client.Dispose();
                                         };
            client.Connect();
        }

        public void SetCredentials(Credentials credentials)
        {
            _Credentials = credentials;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public bool CanFind(string address)
        {
            // If the address contains an ampersand it can't start with it
            // or it just shouldn't have one at all.
            return (!address.StartsWith("@") && address.Contains("@")) || !address.Contains("@");
        }

        public ServiceInformation GetInformation()
        {
            return _ServiceInformation;
        }

        public void StartReceiving()
        {
            //throw new System.NotImplementedException();
        }

        public void StopReceiving()
        {
            //throw new System.NotImplementedException();
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}
