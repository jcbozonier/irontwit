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
        private JabberClient _Client;

        public GoogleTalkDataAccess()
        {
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
                if (OnAuthError != null)
                    OnAuthError(this, null);
            };

            client.OnAuthenticate += s =>
            {

            };

            client.OnMessage += (s, e) =>
            {
                if (OnMessage != null)
                    OnMessage(this, null);
            };

            _Client = client;
        }

        public event EventHandler OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;
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
