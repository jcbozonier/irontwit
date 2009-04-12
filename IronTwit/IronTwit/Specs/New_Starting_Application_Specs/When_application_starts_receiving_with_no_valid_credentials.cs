using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_application_starts_receiving_and_there_are_no_valid_credentials : cached_credentials_no_settings
    {
        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(ReceivingMessagePlugin) });

            FakeRepo.FakeContext
                .Stub(x => x.GetCredentials(FakePlugin.ServiceInformation));

            ViewModel = FakeRepo.GetMainView();
        }

        protected override void Because()
        {
            ViewModel.Init();
        }

        [Test]
        public void It_should_ask_for_credentials()
        {
            FakeRepo.FakeContext
                .AssertWasCalled(x => x.GetCredentials(null));
        }
    }

    public abstract class cached_credentials_no_settings
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
