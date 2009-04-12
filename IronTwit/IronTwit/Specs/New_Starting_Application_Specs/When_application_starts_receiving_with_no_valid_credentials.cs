using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_messaging_service_needs_credentials : cached_credentials_no_settings
    {
        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new[] { typeof(IMessagingService) });

                FakeRepo.FakeContext
                    .Stub(x => x.GetCredentials(null))
                    .Return(FakeRepo.CreateFakeCredentials());

            ViewModel = FakeRepo.GetMainView();
        }

        protected override void Because()
        {
            ViewModel.Init();
        }

        [Test]
        public void It_should_ask_ui_for_credentials()
        {
            FakeRepo.FakeContext
                .AssertWasCalled(x => x.GetCredentials(null));
        }
    }

    public class GetCredentialsPlugIn : FakePlugin
    {
        public override event EventHandler<CredentialEventArgs> CredentialsRequested;

        public override void StartReceiving()
        {
            CredentialsRequested(this, new CredentialEventArgs());
        }
    }

    public abstract class cached_credentials_no_settings
    {
        protected MainView ViewModel;
        protected ScenarioRepository FakeRepo;
        protected IMessagingService FakeMessagingPlugin;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakeMessagingPlugin = new GetCredentialsPlugIn();

            FakeRepo = new ScenarioRepository()
                           {
                               FakeMessagePlugin = FakeMessagingPlugin
                           };

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}
