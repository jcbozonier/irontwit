using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwitterPlugIn;
using NUnit.Framework;
using Unite.Messaging.Services;

namespace Unite.Specs.UnitTests
{
    [Ignore]
    [TestFixture]
    public class ServiceResolverTests
    {
        private const string TwitterAddress = "@darkxanthos";
        private const string EmailAddress = "darkxanthos@gmail.com";
        private const string GChatAddress = "darkxanthos";

        private ServiceResolver _resolver;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _resolver = new ServiceResolver(null);
        }

        [Test]
        public void DetectTwitterAddress()
        {
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(TwitterAddress),
                            "Resolved service for Twitter address");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(null), "Resolved service for null");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(""), "Resolved service for string.Empty");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(" "), "Resolved service for whitespace");
        }

        [Test]
        public void DetectEmailAddress()
        {
            try
            {
                _resolver.GetService(EmailAddress);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(NotSupportedException), ex.GetType(), "Email is not supported");
                Assert.AreEqual("Email", ex.Message, "Service not supported");
                return;
            }
            Assert.Fail("Should have thrown NotSupported Exception");
        }

        [Test]
        public void DetectGChatAddress()
        {
            try
            {
                _resolver.GetService(GChatAddress);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(NotSupportedException), ex.GetType(), "GChat is not supported");
                Assert.AreEqual("GChat", ex.Message, "Service not supported");
                return;
            }
            Assert.Fail("Should have thrown NotSupported Exception");
        }
    }
}
