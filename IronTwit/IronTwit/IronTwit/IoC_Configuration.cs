using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.Models;
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
            });

        }

    }

}
