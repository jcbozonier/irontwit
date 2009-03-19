using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwitterPlugIn;
using NUnit.Framework;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using IServiceProvider=Unite.Messaging.Services.IServiceProvider;

namespace Unite.Specs.UnitTests
{
    //[Ignore]
    [TestFixture]
    public class ServiceResolverTests
    {
        private const string TwitterAddress = "@darkxanthos";
        private const string EmailAddress = "darkxanthos@gmail.com";
        private const string GChatAddress = "darkxanthos";

        private ServiceResolver _resolver;

        [Test]
        public void DetectTwitterAddress()
        {
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(TwitterAddress).ServiceID,
                            "Resolved service for Twitter address");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(null).ServiceID, "Resolved service for null");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService("").ServiceID, "Resolved service for string.Empty");
            Assert.AreEqual(TwitterUtilities.SERVICE_ID, _resolver.GetService(" ").ServiceID, "Resolved service for whitespace");
        }

        [Test]
        public void DetectEmailAddress()
        {
            Assert.AreEqual(null, _resolver.GetService(EmailAddress), "Provider for Email addresses (not implemented yet)");
        }

        [Test]
        public void DetectGChatAddress()
        {
            Assert.AreEqual(null, _resolver.GetService(GChatAddress), "Provider for GChat addresses (not implemented yet)");
        }

        internal class TestServiceProvider : IServiceProvider
        {
            private List<IMessagingService> _services = new List<IMessagingService>();

            public void Add(params IMessagingService[] services)
            {
                _services.AddRange(services);
            }

            public IEnumerable<IMessagingService> GetServices()
            {
                return _services;
            }

            public event EventHandler<CredentialEventArgs> CredentialsRequested;
            public IMessagingService GetService(ServiceInformation info)
            {
                return _services.FirstOrDefault(service => service.GetInformation().Equals(info));
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            IServiceProvider serviceProvider = new TestServiceProvider();
            serviceProvider.Add(new TwitterUtilities());
            _resolver = new ServiceResolver(serviceProvider);
        }
    }
}
