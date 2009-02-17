using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronTwit.Models
{
    public class Tweet
    {
        public string text { get; set; }
        public TwitUser user { get; set; } 
    }
}
