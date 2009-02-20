using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronTwit.Models.Twitter;
using Newtonsoft.Json;
using StructureMap;
using Yedda;

namespace IronTwit.Utilities
{
    public interface ITwitterUtilities
    {
        List<Tweet> GetUserMessages(string username, string password);
        void SendMessage(string username, string password, string message, string recipient);
    }

    public interface ITwitterDataAccess
    {
        /// <summary>
        /// Returns a status in JSON.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string SendMessage(string username, string password, string message);
        string GetFriendsTimelineAsJSON(string username, string password);
    }

    [StructureMap.Pluggable("Complex")]
    public class TwitterUtilities : ITwitterUtilities
    {
        private readonly int MaxMessageLength = 140;
        private ITwitterDataAccess _DataAccess;

        /// <summary>
        /// This constructor should only be used if Twitter changes their default max
        /// message length. Until that time, this is only for testing.
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="maxMessageLength"></param>
        public TwitterUtilities(ITwitterDataAccess dataAccess, int maxMessageLength)
            : this(dataAccess)
        {
            MaxMessageLength = maxMessageLength;
        }
        
        //Stupid StructureMap kruft. >:(
        [DefaultConstructor]
        public TwitterUtilities(ITwitterDataAccess dataAccess)
        {
            // .NET Twitter fix for HTTP Error 417 with Twitter
            System.Net.ServicePointManager.Expect100Continue = false;

            _DataAccess = dataAccess;
        }

        public void SendMessage(string username, string password, string message, string recipient)
        {
            //1 is for the space and should only be subtracted if there is a recipient
            var maxLengthOfMessageBody = MaxMessageLength - recipient.Length - 1; 

            if(maxLengthOfMessageBody <= 0)
                throw new ArgumentException("The recipient's name is too long for the given message limit.");

            if(message.Length > maxLengthOfMessageBody)
            {
                // Find out how many times maxLengthOfMessageBody divides into message.Length
                // Loop that many times plus one
                // in each iteration remove a set of characters of length maxLengthOfMessageBody
                // concatenate the recipient's name and the message portion
                // then send the message
                // continue

                var numberOfMessagesToSend = message.Length/maxLengthOfMessageBody;

                for(var count=0; count<=numberOfMessagesToSend; count++)
                {
                    string messageToSend = _GetMessageToSend(recipient, maxLengthOfMessageBody, message);
                    messageToSend = (String.IsNullOrEmpty(recipient))
                                ? messageToSend
                                : String.Format("{0} {1}", recipient, messageToSend);
                    message = _CropMessage(maxLengthOfMessageBody, message);

                    _DataAccess.SendMessage(username, password, messageToSend);
                }

                return;
            }

            _DataAccess.SendMessage(username, password, message);
        }

        private string _CropMessage(int maxLengthOfMessageBody, string message)
        {
            var numberOfCharactersToGrab = _GetNumberOfCharactersToGrab(message, maxLengthOfMessageBody);

            message = message.Remove(0, numberOfCharactersToGrab);
            return message;
        }

        public static string _GetMessageToSend(string recipient, int maxLengthOfMessageBody, string message)
        {
            int numberOfCharactersToGrab = _GetNumberOfCharactersToGrab(message, maxLengthOfMessageBody);

            var messageToSend = message.Substring(0, numberOfCharactersToGrab);

            return messageToSend;
        }

        public static int _GetNumberOfCharactersToGrab(string message, int maxLengthOfMessageBody)
        {
            var result = message.Length > maxLengthOfMessageBody
                       ? maxLengthOfMessageBody
                       : message.Length;

            var croppedMessage = message.Substring(0, result).TrimEnd(' ');

            return croppedMessage.Length;
        }

        public List<Tweet> GetUserMessages(string username, string password)
        {
            string resultString = String.Empty;

            resultString = _DataAccess.GetFriendsTimelineAsJSON(username, password);

            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            return (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
        }
    }
}
