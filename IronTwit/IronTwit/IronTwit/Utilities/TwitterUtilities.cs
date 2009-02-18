using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronTwit.Models.Twitter;
using Newtonsoft.Json;
using Yedda;

namespace IronTwit.Utilities
{
    public interface ITwitterUtilities
    {
        List<Tweet> GetUserMessages(string username, string password);
        void SendMessage(string username, string password, string message, string recipient);
    }

    public class TwitterUtilities : ITwitterUtilities
    {
        public void SendMessage(string username, string password, string message, string recipient)
        {
            
        }

        public List<Tweet> GetUserMessages(string username, string password)
        {
            var twit = new Twitter();

            var resultString = twit.GetFriendsTimeline(username, password, Twitter.OutputFormatType.JSON);
            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            return (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
        }
    }
}
