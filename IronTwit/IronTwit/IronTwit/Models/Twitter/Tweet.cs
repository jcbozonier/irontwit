using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronTwit.Models.Twitter
{
    public class Tweet
    {
        public string text { get; set; }
        public TwitterUser user { get; set; } 
    }
}