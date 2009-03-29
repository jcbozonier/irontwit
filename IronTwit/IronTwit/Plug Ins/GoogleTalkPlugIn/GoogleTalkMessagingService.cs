using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jabber.client;
using jabber.protocol.client;
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
        private CredentialEventArgs _CredEventArgs;

        private static readonly ServiceInformation _ServiceInformation = new ServiceInformation()
                                                             {
                                                                 ServiceID = Guid.NewGuid(),
                                                                 ServiceName = "GoogleTalk"
                                                             };

        public GoogleTalkMessagingService()
        {
            _CredEventArgs = new CredentialEventArgs()
                                 {
                                     ServiceInfo = _ServiceInformation
                                 };

            var client = new JabberClient();

            client.AutoPresence = true;
            client.AutoRoster = false;
            client.AutoReconnect = 1;

            client.Server = "gmail.com";
            client.Port = 5222;
            client.Resource = "Unit3";

            client.OnError += (s, e) =>
            {
                throw new Exception(e.Message);
            };


            client.OnAuthError += (s, e) =>
            {
                if (AuthorizationFailed != null)
                    AuthorizationFailed(this, _CredEventArgs);
            };

            client.OnAuthenticate += s =>
            {

            };

            client.OnMessage += (s, e) =>
            {
                var user = new GTalkUser()
                {
                    ServiceInfo = _ServiceInformation,
                    UserName = e.From.User
                };
                var messageReceived = new GTalkMessage()
                {
                    Address = user,
                    Text = e.Body
                };
                if (MessagesReceived != null)
                    MessagesReceived(
                        this,
                        new MessagesReceivedEventArgs(new[] { messageReceived }));
            };

            _Client = client;
        }

        public bool CanAccept(Credentials credentials)
        {
            return !String.IsNullOrEmpty(credentials.UserName) && credentials.ServiceInformation == _ServiceInformation;
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            _AuthenticateIfNeeded();
            
            if(_Credentials == null)
                throw new Exception("Your credentials can not still be null. This should NEVER happen.");

            _Client.Message(recipient.UserName, message);
        }

        public void SetCredentials(Credentials credentials)
        {
            _Credentials = credentials;
            _Client.User = _Credentials.UserName;
            _Client.Password = _Credentials.Password;
        }

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
            _AuthenticateIfNeeded();
            _IsConnected = true;
        }

        private void _AuthenticateIfNeeded()
        {
            if(String.IsNullOrEmpty(_Client.User))
            {
                if (CredentialsRequested != null)
                    CredentialsRequested(this, _CredEventArgs);
                _Client.Connect();
            }   
        }

        public void StopReceiving()
        {
            _Client.Close();
        }

        private bool _IsConnected;

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}
