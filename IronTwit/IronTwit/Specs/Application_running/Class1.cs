using System;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging.Messages;
using Unite.UI.ViewModels;

namespace Unite.Specs.Application_running.Managing_Plugins
{
    [TestFixture]
    public class When_a_message_is_received_while_messages_already_exist : context
    {
        [Test]
        public void It_should_increase_the_number_of_viewable_messages_by_one()
        {
            Model.Messages.Count.ShouldEqual(2);
        }

        [Test]
        public void It_should_place_the_most_recent_message_at_the_top_of_the_list()
        {
            Model.Messages[0].TimeStamp.CompareTo(Model.Messages[1].TimeStamp).ShouldEqual(1);
        }

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
