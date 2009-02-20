using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Utilities;
using NUnit.Framework;
using SpecUnit;

namespace Specs.UnitTests
{
    [TestFixture]
    public class Formatting_messages_for_twitter
    {
        [Test]
        public void TestOne()
        {
            var finalMessage = "@darkxanthos This";
            //var result = TwitterUtilities._GetMessageToSend(finalMessage.Length, "This is my message.");
            
            //result.ShouldEqual(finalMessage);
        }
    }
}
