using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Utilities;
using IronTwitterPlugIn;
using StructureMap;
using UniteMessaging;

namespace Specs
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
                x.ForRequestedType<ITwitterDataAccess>().TheDefaultIsConcreteType<TwitterDataAccess>();
                x.ForRequestedType<IMessagingService>().TheDefault.Is.OfConcreteType<TwitterUtilities>();
            });

        }

    }

}
