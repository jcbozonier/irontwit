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
        public TwitterUtilities()
        {
            // .NET Twitter fix for HTTP Error 417 with Twitter
            System.Net.ServicePointManager.Expect100Continue = false;
        }

        public void SendMessage(string username, string password, string message, string recipient)
        {
            var twit = new Twitter();

            message = (String.IsNullOrEmpty(recipient))
                          ? message
                          : String.Format("{0} {1}", recipient, message);

            var result = twit.UpdateAsJSON(username, password, message);
        }

        public List<Tweet> GetUserMessages(string username, string password)
        {
            var twit = new Twitter();
            string resultString = String.Empty;

            try
            {
                resultString = twit.GetFriendsTimeline(username, password, Twitter.OutputFormatType.JSON);
            }
            catch(Exception e)
            {
                var a = 1;
            }

            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            return (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
        }
    }
}
