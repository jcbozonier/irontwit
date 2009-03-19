using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;

namespace Unite.Specs.Application_running
{
    [TestFixture]
    public abstract class context
    {
        protected MainView Model;
        protected TestTwitterUtilities Utilities;


        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();

            Utilities = new TestTwitterUtilities();
            ObjectFactory.EjectAllInstancesOf<IMessagingService>();
            ObjectFactory.Inject<IMessagingService>(Utilities);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}
