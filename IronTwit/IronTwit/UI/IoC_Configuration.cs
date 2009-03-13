using Unite.UI.Utilities;
using StructureMap;
using Unite.Messaging;

namespace Unite.UI
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IServiceProvider>().TheDefaultIsConcreteType<ServiceProvider>();
            });
        }
    }
}
