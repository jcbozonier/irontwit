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
            // This should be the only place in the project with a reference to Twitter.
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<IMessagingService>().TheDefaultIsConcreteType<NullObjectMessageService>();
                x.ForRequestedType<IMessage>().TheDefaultIsConcreteType<NullObjectMessage>();
            });
        }
    }
}
