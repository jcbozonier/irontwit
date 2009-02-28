using IronTwitterPlugIn.DataObjects;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unite.Messaging;
using Yedda;

namespace IronTwitterPlugIn
{
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
        string GetMessages(string username, string password);
    }

    [StructureMap.Pluggable("Complex")]
    public class TwitterUtilities : IMessagingService
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
            // Get the maximum length of a message subtracting the to field
            // go to the maxLength - 1 index of the message and search backwards for a space or end of string.
            // tag this index as the end index
            // pull message from start index to end index into message variable
            // tag on the recipient
            // send message
            // repeat until the endIndex == messageLength-1.

            var maxLengthOfMessageContent = (!String.IsNullOrEmpty(recipient))
                                                ? 140 - (recipient.Length + 1) //For space below
                                                : 140;

            var recipientMessagePortion = (!String.IsNullOrEmpty(username))
                                              ? recipient + " "
                                              : "";

            var startMessageIndex = 0;
            var endMessageIndex = Math.Min(maxLengthOfMessageContent - 1, message.Length-1);

            var messagesToSend = new List<string>();

            while (startMessageIndex < message.Length - 1)
            {
                if(startMessageIndex >= message.Length ||
                    startMessageIndex > endMessageIndex)
                    break;

                if( message[endMessageIndex] == ' ' || 
                    endMessageIndex >= message.Length-1)
                {
                    // Then this is the message portion we want.
                    messagesToSend.Add(
                        recipientMessagePortion + message.Substring(startMessageIndex, endMessageIndex - startMessageIndex + 1));

                    startMessageIndex = endMessageIndex + 1;
                    endMessageIndex = Math.Min(startMessageIndex + maxLengthOfMessageContent - 1, message.Length-1);
                }
                else
                {
                    endMessageIndex--;
                }
                
            }

            messagesToSend.ForEach((messageToSend) => _DataAccess.SendMessage(username, password, messageToSend));
        }

        public List<IMessage> GetMessages(string username, string password)
        {
            string resultString = String.Empty;
            
            resultString = _DataAccess.GetMessages(username, password);

            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;
            return new List<IMessage>(((List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>))).ToArray());
        }
    }
}
