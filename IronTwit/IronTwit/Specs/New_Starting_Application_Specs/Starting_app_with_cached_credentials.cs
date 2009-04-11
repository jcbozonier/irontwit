using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;
using IServiceProvider=Unite.Messaging.Services.IServiceProvider;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_application_starts_with_cached_credentials : using_settings_provider_with_a_credential_in_it
    {
        private bool AskedForCredentials;

        [Ignore]
        [Test]
        public void It_should_login_using_previously_saved_credentials()
        {
            SavedCredentials.ShouldEqual(ExpectedCredentials);
        }

        [Test]
        public void It_should_NOT_ask_for_credentials()
        {
            AskedForCredentials.ShouldBeFalse();
        }

        protected override void Because()
        {
            ViewModel.Init();
        }

        protected override void Context()
        {
            FakeMessagingService
                .Stub(x => x.SetCredentials(null))
                .IgnoreArguments()
                .Callback(
                    (Func<Credentials, bool>)(
                        y =>
                            {
                                SavedCredentials = y;
                                return true;
                            }));

            RealServicesManager.CredentialsRequested += (s, e) => AskedForCredentials = true;

            ViewModel = ObjectFactory.GetInstance<MainView>();

        }
    }

    public abstract class using_settings_provider_with_a_credential_in_it
    {
        protected MainView ViewModel;
        protected IInteractionContext FakeGui;
        protected IMessagingService FakeMessagingService;
        protected IServiceProvider FakeServiceProvider;
        protected ServicesManager RealServicesManager;

        protected Credentials SavedCredentials;
        protected Credentials ExpectedCredentials;

        [TestFixtureSetUp]
        public void Setup()
        {
            var messageInfo = new ServiceInformation(){ServiceID = Guid.NewGuid(), ServiceName = "TestService"};
            ExpectedCredentials = new Credentials() {ServiceInformation = messageInfo};

            FakeGui = MockRepository.GenerateStub<IInteractionContext>();
            FakeMessagingService = MockRepository.GenerateStub<IMessagingService>();
            //FakeMessagingService.Stub(x => x.StartReceiving()).Throw;()

            FakeServiceProvider = MockRepository.GenerateStub<IServiceProvider>();
            FakeServiceProvider.Stub(x => x.GetServices()).Return(new[] { FakeMessagingService });

            RealServicesManager = new ServicesManager(FakeServiceProvider);
            
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefault.IsThis(FakeGui);
                x.ForRequestedType<IMessagingServiceManager>().TheDefault.IsThis(RealServicesManager);
                x.ForRequestedType<IMessagingService>().TheDefault.IsThis(FakeMessagingService);
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
                x.ForRequestedType<IServiceProvider>().TheDefault.IsThis(FakeServiceProvider);
            });

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}