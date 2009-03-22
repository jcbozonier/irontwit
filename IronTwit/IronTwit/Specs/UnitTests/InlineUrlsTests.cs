using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Unite.Specs.UnitTests
{
    [TestFixture]
    public class InlineUrlsTests
    {
        [Test]
        public void SimpleUrlAtBeginningOfMessage()
        {
            var url = "http://wwww.yahoo.com";
            var message = string.Format("{0} is a cool site!", url);
            var inlineUris = UI.Utilities.InlineUris.Get(message);
            Assert.AreEqual(1, inlineUris.Count, "Number of InlineUris returned");
            var inlineUri = inlineUris[0];
            Assert.AreEqual(0, inlineUri.StartIndex, "Start index of InlineUri returned");
            Assert.AreEqual(url.Length, inlineUri.Length, "Lenth of InlineUri returned");
            Assert.AreEqual(url, inlineUri.OriginalString, "InlineUri original string");
        }

        [Test]
        public void SimpleUrlInMiddleOfMessage()
        {
            var url = "http://www.yahoo.com";
            var message = string.Format("@darkxanthos hey, {0} is a cool site!", url);
            var inlineUris = UI.Utilities.InlineUris.Get(message);
            Assert.AreEqual(1, inlineUris.Count, "Number of InlineUris returned");
            var inlineUri = inlineUris[0];
            Assert.AreEqual(message.IndexOf(url, 0), inlineUri.StartIndex, "Start index of InlineUri returned");
            Assert.AreEqual(url.Length, inlineUri.Length, "Lenth of InlineUri returned");
            Assert.AreEqual(url, inlineUri.OriginalString, "InlineUri original string");
        }

        [Test]
        public void SimpleUrlAtEndOfMessage()
        {
            var url = "http://www.yahoo.com";
            var message = string.Format("@darkxanthos hey, I think you should check out this cool site - {0}", url);
            var inlineUris = UI.Utilities.InlineUris.Get(message);
            Assert.AreEqual(1, inlineUris.Count, "Number of InlineUris returned");
            var inlineUri = inlineUris[0];
            Assert.AreEqual(message.IndexOf(url, 0), inlineUri.StartIndex, "Start index of InlineUri returned");
            Assert.AreEqual(url.Length, inlineUri.Length, "Lenth of InlineUri returned");
            Assert.AreEqual(url, inlineUri.OriginalString, "InlineUri original string");
        }

        [Test]
        public void TwoUrlsInMiddleAndAtEnd()
        {
            var urlA = "http://www.yahoo.com";
            var urlB = "http://www.microsoft.com";
            var message = string.Format("This place {0} really would have been better off going to this place {1}", urlA,
                                        urlB);
            var inlineUris = UI.Utilities.InlineUris.Get(message);
            Assert.AreEqual(2, inlineUris.Count, "Number of InlineUris returned");
            var inlineUriA = inlineUris[0];
            var inlineUriB = inlineUris[1];
            Assert.AreEqual(message.IndexOf(urlA, 0), inlineUriA.StartIndex, "Start index of first InlineUri");
            Assert.AreEqual(urlA.Length, inlineUriA.Length, "Lenght of first InlineUri");
            Assert.AreEqual(urlA, inlineUriA.OriginalString, "First InlineUri original string");
            Assert.AreEqual(message.IndexOf(urlB, 0), inlineUriB.StartIndex, "Start index of second InlineUri");
            Assert.AreEqual(urlB.Length, inlineUriB.Length, "Length of second InlineUri");
            Assert.AreEqual(urlB, inlineUriB.OriginalString, "Second InlineUri original string");
        }

        [Test]
        public void NullOrEmptyMessageText()
        {
            string text = null;
            var inlineUris = UI.Utilities.InlineUris.Get(text);
            Assert.AreEqual(0, inlineUris.Count, "Count of Inline Uris for null text");
            text = "";
            inlineUris = UI.Utilities.InlineUris.Get(text);
            Assert.AreEqual(0, inlineUris.Count, "Count of Inline Uris for empty string text");
        }
    }
}
