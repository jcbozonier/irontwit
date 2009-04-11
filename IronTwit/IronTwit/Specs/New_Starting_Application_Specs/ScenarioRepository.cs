using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Messages;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    public static class ScenarioRepository
    {
        static ScenarioRepository()
        {
            ContainerBootstrapper.BootstrapStructureMap();
        }

        public static MainView GetMainView()
        {
            return ObjectFactory.GetInstance<MainView>();
        }
    }

    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {
            var mock = new Rhino.Mocks.MockRepository();
            var fakeGui = mock.Stub<IInteractionContext>();
            var fakeMessageServiceManager = mock.Stub<IMessagingServiceManager>();
            var fakeMessageService = mock.Stub<IMessagingService>();

            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefault.IsThis(fakeGui);
                x.ForRequestedType<IMessagingServiceManager>().TheDefault.IsThis(fakeMessageServiceManager);
                x.ForRequestedType<IMessagingService>().TheDefault.IsThis(fakeMessageService);
                x.ForRequestedType<IContactProvider>().TheDefaultIsConcreteType<ContactProvider>();
            });

        }

    }
}
