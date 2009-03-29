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
        private readonly GoogleTalkDataAccess _DataAccess;

        private Credentials _Credentials;
        private readonly CredentialEventArgs _CredEventArgs;

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

            _DataAccess = new GoogleTalkDataAccess();
            _DataAccess.OnMessage += _DataAccess_OnMessage;
            _DataAccess.OnAuthError += _DataAccess_OnAuthError;
        }

        void _DataAccess_OnAuthError(object sender, EventArgs e)
        {
            if (AuthorizationFailed != null)
                AuthorizationFailed(this, _CredEventArgs);
        }

        void _DataAccess_OnMessage(object sender, GTalkMessageEventArgs e)
        {
            _ReceiveMessage(e.User ?? _Credentials.UserName, e.Message);
        }

        private void _ReceiveMessage(string username, string message)
        {
            var user = new GTalkUser()
                           {
                               ServiceInfo = _ServiceInformation,
                               UserName = username
                           };
            var messageReceived = new GTalkMessage()
                                      {
                                          Address = user,
                                          Text = message
                                      };
            if (MessagesReceived != null)
                MessagesReceived(
                    this,
                    new MessagesReceivedEventArgs(new[] { messageReceived }));
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

            var realUsername = !recipient.UserName.ToLowerInvariant().EndsWith("@gmail.com")
                                   ? recipient.UserName + "@gmail.com"
                                   : recipient.UserName;

            _DataAccess.Message(realUsername, message);
        }

        public void SetCredentials(Credentials credentials)
        {
            _Credentials = credentials;
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
        }

        private void _AuthenticateIfNeeded()
        {
            if(!_DataAccess.IsConnected)
            {
                if (CredentialsRequested != null)
                    CredentialsRequested(this, _CredEventArgs);
                _DataAccess.Login(_Credentials.UserName, _Credentials.Password);
            }   
        }

        public void StopReceiving()
        {
            _DataAccess.Logoff();
        }

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}
