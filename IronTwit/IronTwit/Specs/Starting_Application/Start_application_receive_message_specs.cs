﻿using System;
using System.Collections.Generic;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;

namespace Unite.Specs.Application_starting.Receiving_messages
{
    [TestFixture]
    public class When_refresh_is_requested : context
    {
        [Test]
        public void It_should_get_items()
        {
            Model.Messages.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void It_should_be_able_to_refresh()
        {
            Model.ReceiveMessage.CanExecute(null).ShouldBeTrue();
        }

        protected override void Because()
        {
            Model.ReceiveMessage.Execute(null);
        }

        protected override void Context()
        {
            Model.Init();
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;

        protected bool Message_was_sent
        {
            get;
            set;
        }


        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();
            Model = ObjectFactory.GetInstance<MainView>();
            
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestDataAccess : ITwitterDataAccess
    {
        public string SendMessage(Credentials credentials, string message)
        {
            return "";
        }

        public string GetMessages(Credentials credentials)
        {
            return "[{text:'Message 1'}]";
        }
    }

    public class TestingInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            return new Credentials()
                       {
                           UserName = "testuser",
                           Password = "testpw",
                           ServiceInformation = serviceInformation
                       };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            return false;
        }
    }

    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {

            var serviceProviders = new ServiceProvider();
            serviceProviders.Add(new TwitterUtilities(new TestDataAccess()));

            // Initialize the static ObjectFactory container

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<TestingInteractionContext>();
                x.ForRequestedType<IMessagingServiceManager>().TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<Messaging.IServiceProvider>().TheDefault.IsThis(serviceProviders);
            });

        }

    }
}
