using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.Specs.Application_running
{
    [TestFixture]
    public class When_clicking_on_a_message : message_has_been_received
    {
        private string Sender;

        [Ignore]
        [Test]
        public void It_should_show_the_message_sender_in_the_recipient_field()
        {
            Model.Recipient.ShouldEqual(Sender);
        }

        protected override void Because()
        {
            Model.SelectedMessage = RecievedMessage;
        }

        protected override void Context()
        {
            Sender = RecievedMessage.Address.UserName;
            this.Model.ReceiveMessage.Execute(null);
        }
    }

    public abstract class message_has_been_received
    {
        protected MainView Model;
        protected UiMessage RecievedMessage;

        [TestFixtureSetUp]
        public void Setup()
        {
            var mocks = new MockRepository();
            var messagingManager = mocks.DynamicMock<IMessagingServiceManager>();
            using(mocks.Record())
            {
                
            }
            ContainerBootstrapper.BootstrapStructureMap();
            Model = ObjectFactory.GetInstance<MainView>();
            ObjectFactory.EjectAllInstancesOf<IMessagingServiceManager>();
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class Message : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IIdentity Address
        {
            get; set;
        }
    }
}
