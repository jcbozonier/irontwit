using Unite.UI.Utilities;
using IronTwitterPlugIn;
using StructureMap;
using Unite.Messaging;

namespace Unite.UI
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            var serviceProviders = new ServiceProvider();
            serviceProviders.Add(new TwitterUtilities(new TwitterDataAccess()));

            // Initialize the static ObjectFactory container
            // This should be the only place in the project with a reference to Twitter.
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<Views.MainView>().TheDefaultIsConcreteType<Views.MainView>();
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<ServicesManager>();
                x.ForRequestedType<IServiceProvider>().TheDefault.IsThis(serviceProviders);
            });
        }
    }
}
