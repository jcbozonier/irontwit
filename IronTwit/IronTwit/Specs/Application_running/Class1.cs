using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.UI.ViewModels;

namespace Unite.Specs.Application_running.Managing_Plugins
{
    [TestFixture]
    public class Add_Existing_Plugins_Service : context
    {
        protected override void Context()
        {
            Model = ObjectFactory.GetInstance<MainView>();
            Model.Init();

            Model.Messages.Count.ShouldEqual(1);
        }

        protected override void Because()
        {
            Model.ReceiveMessage.Execute(null);
        }

    }

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
