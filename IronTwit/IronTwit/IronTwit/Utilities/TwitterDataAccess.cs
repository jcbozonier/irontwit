using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yedda;

namespace IronTwit.Utilities
{
    public class TwitterDataAccess : ITwitterDataAccess
    {
        public string SendMessage(string username, string password, string message)
        {
            var twit = new Twitter();
            var result = twit.UpdateAsJSON(username, password, message);
            return result;
        }

        public string GetMessages(string username, string password)
        {
            var twit = new Twitter();
            var result = twit.GetFriendsTimelineAsJSON(username, password);
            return result;
        }
    }
}
