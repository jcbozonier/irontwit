﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecUnit;
using Unite.Messaging;

namespace Unite.Specs.Using_Services
{
    [TestFixture]
    public class When_getting_messages : context
    {
        [Test]
        public void It_should_get_messages_from_all_available_services()
        {
            Messages.Count().ShouldEqual(2);
        }

        protected override void Because()
        {
            Messages = ServiceManager.GetMessages(MyCredentials);
        }

        protected override void Context()
        {
            
        }
    }

    public abstract class context
    {
        protected IMessagingService ServiceManager;
        protected ServiceProvider ServiceProvider;
        protected IEnumerable<IMessage> Messages;
        protected Credentials MyCredentials;

        [TestFixtureSetUp]
        public void Setup()
        {
            MyCredentials = new Credentials() { UserName = "username", Password = "password" };
            ServiceProvider = new ServiceProvider(new FauxMessageService("test 1"), new FauxMessageService("test2"));
            ServiceManager = new ServicesManager(ServiceProvider);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class FauxMessageService : IMessagingService
    {
        private string MessageText;

        public FauxMessageService(string messageText)
        {
            MessageText = messageText;
        }

        public List<IMessage> GetMessages(Credentials credentials)
        {
            return new List<IMessage>()
                       {
                           new Message()
                               {
                                   Recipient = new Recipient(){UserName = "darkxanthos"},
                                   Text = MessageText
                               }
                       };
        }

        public void SendMessage(Credentials credentials, string recipient, string message)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Recipient : IRecipient
    {
        public string UserName
        {
            get; set;
        }
    }

    public class Message : IMessage
    {
        public string Text
        {
            get; set;
        }

        public IRecipient Recipient
        {
            get; set;
        }
    }
}