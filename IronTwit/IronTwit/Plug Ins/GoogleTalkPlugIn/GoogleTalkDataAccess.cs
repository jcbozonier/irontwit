using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jabber.client;
using jabber.protocol.client;

namespace GoogleTalkPlugIn
{
    public class GoogleTalkDataAccess
    {
        private readonly JabberClient _Client;
        public bool IsConnected;

        public GoogleTalkDataAccess()
        {
            var client = new JabberClient();

            client.AutoLogin = true;
            client.AutoPresence = true;
            client.AutoRoster = false;
            client.AutoReconnect = 1;

            client.Server = "gmail.com";
            client.Port = 5222;
            client.Resource = "Unit3";

            client.OnConnect += (s, e) =>
                                    {
                                        IsConnected = e.Connected;
                                    };

            client.OnError += (s, e) =>
            {
                throw new Exception(e.Message);
            };
            client.OnAuthError += (s, e) => OnAuthError(s, null);
            client.OnMessage += (s, e) =>
                                    {
                                        OnMessage(s, new GTalkMessageEventArgs(e.From.User, e.Body));
                                    };

            _Client = client;
        }

        public event EventHandler OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;

        public void Message(string name, string message)
        {
            _Client.Message(name, message);
        }

        public void Login(string name, string password)
        {
            _Client.User = name;
            _Client.Password = password;
            _Connect();
        }

        private void _Connect()
        {
            _Client.Connect();
        }

        internal void Logoff()
        {
            _Client.Close();
        }
    }

    public class GTalkMessageEventArgs : EventArgs
    {
        public GTalkMessageEventArgs(string user, string message)
        {
            Message = message;
            User = user;
        }

        public string Message;
        public string User;
    }
}
