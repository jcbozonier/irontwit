using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Messages;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_application_starts : no_cached_credentials_no_settings
    {
        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(IMessagingService) });

            ViewModel = FakeRepo.GetMainView();
        }

        protected override void Because()
        {
            ViewModel.Init();
        }

        [Test]
        public void It_should_start_receiving_messages()
        {
            FakeRepo.FakeMessagePlugin.AssertWasCalled(x => x.StartReceiving());
        }
    }

    public abstract class no_cached_credentials_no_settings
    {
        protected MainView ViewModel;
        protected ScenarioRepository FakeRepo;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakeRepo = new ScenarioRepository();
            
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}