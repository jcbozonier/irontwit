using IronTwitterPlugIn.DataObjects;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace IronTwitterPlugIn
{
    public interface ITwitterDataAccess
    {
        /// <summary>
        /// Returns a status in JSON.
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string SendMessage(Credentials credentials, string message);
        string GetMessages(Credentials credentials);
    }

    [StructureMap.Pluggable("Complex")]
    public class TwitterUtilities : IMessagingService
    {
        private readonly int MaxMessageLength = 140;
        private ITwitterDataAccess _DataAccess;

        public static readonly Guid SERVICE_ID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B565D7}");
        public static readonly string SERVICE_NAME = "Twitter";

        public Guid ServiceId { get { return SERVICE_ID; } }
        public string ServiceName { get { return SERVICE_NAME; } }

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
        
        [DefaultConstructor]
        public TwitterUtilities() : this(null)
        {
            
        }

        public TwitterUtilities(ITwitterDataAccess dataAccess)
        {
            // .NET Twitter fix for HTTP Error 417 with Twitter
            System.Net.ServicePointManager.Expect100Continue = false;

            _DataAccess = dataAccess; //testing path
            if (dataAccess == null) //default path
                _DataAccess = new TwitterDataAccess();
        }

        public void SendMessage(Credentials credentials, string recipient, string message)
        {
            // Get the maximum length of a message subtracting the to field
            // go to the maxLength - 1 index of the message and search backwards for a space or end of string.
            // tag this index as the end index
            // pull message from start index to end index into message variable
            // tag on the recipient
            // send message
            // repeat until the endIndex == messageLength-1.

            var maxLengthOfMessageContent = (!string.IsNullOrEmpty(recipient))
                                                ? 140 - (recipient.Length + 1) //For space below
                                                : 140;

            var recipientMessagePortion = (!String.IsNullOrEmpty(credentials.UserName))
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

            messagesToSend.ForEach((messageToSend) => _DataAccess.SendMessage(credentials, messageToSend));
        }

        public List<IMessage> GetMessages(Credentials credentials)
        {
            string resultString = String.Empty;
            
            resultString = _DataAccess.GetMessages(credentials);

            var str = new StringReader(resultString);
            var converter = new JsonSerializer();
            converter.MissingMemberHandling = MissingMemberHandling.Ignore;

            // Convert the sender property to proper twitter form.
            var tweets = (List<Tweet>)converter.Deserialize(str, typeof(List<Tweet>));
            //tweets.ForEach(tweet=>tweet.Sender.UserName = "@" + tweet.Sender.UserName);

            return new List<IMessage>(tweets.ToArray());
        }
    }
}
