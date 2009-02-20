using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Models;
using IronTwit.Utilities;
using StructureMap;

namespace Specs
{
    public static class ContainerBootstrapper
    {

        public static void BootstrapStructureMap()
        {

            // Initialize the static ObjectFactory container

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<GuiInteractionContext>();
                x.ForRequestedType<ITwitterDataAccess>().TheDefaultIsConcreteType<TwitterDataAccess>();
                x.ForRequestedType<ITwitterUtilities>().TheDefault.Is.OfConcreteType<TwitterUtilities>();
            });

        }

    }

}
