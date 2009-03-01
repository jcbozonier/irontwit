using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Unite.UI.Utilities
{
    public static class InlineUris
    {
        //http://www.regexguru.com/2008/11/detecting-urls-in-a-block-of-text/
        private const string URI_REGEX =
            @"\b(?:(?:https?|ftp|file)://|www\.|ftp\.)"
          + @"(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*"
          + @"(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])";// (free-spacing, case insensitive)

        private static Regex _regex = new Regex(URI_REGEX, RegexOptions.IgnoreCase);

        public static List<InlineUri> Get(string text)
        {
            var uris = new List<InlineUri>();

            Match match;
            int index = 0;
            while (true)
            {
                match = _regex.Match(text, index);
                if (match == Match.Empty)
                    break;

                uris.Add(new InlineUri(text.Substring(match.Index, match.Length), match.Index, match.Length));
                index = match.Index + match.Length;
            };

            return uris;
        }
    }

    public class InlineUri : Uri
    {
        public int StartIndex { get; private set; }
        public int Length { get; private set; }

        public InlineUri(string uriString, int startIndex, int length) : base(uriString)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}
