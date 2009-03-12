using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;
using Yedda;

namespace IronTwitterPlugIn
{
    public class TwitterDataAccess : ITwitterDataAccess
    {
        public string SendMessage(Credentials credentials, string message)
        {
            var twit = new Twitter();
            var result = twit.UpdateAsJSON(credentials.UserName, credentials.Password, message);
            return result;
        }

        public string GetMessages(Credentials credentials)
        {
            var twit = new Twitter();
            var result = twit.GetFriendsTimelineAsJSON(credentials.UserName, credentials.Password);
            return result;
        }
    }
}
