using System;
using System.Collections.Generic;

namespace Unite.Messaging.Services
{
    public interface IPluginFinder
    {
        IEnumerable<Type> GetAllPlugins();
    }
}