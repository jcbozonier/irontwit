using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class PluginFinder : IPluginFinder
    {
        public IEnumerable<Type> GetAllPlugins()
        {
            var mainExeDir = Environment.CurrentDirectory;
            var pluginDir = new DirectoryInfo(mainExeDir);
            var thisAssembly = Assembly.GetExecutingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();

            var result = new List<Type>();

            if (entryAssembly == null) return new List<Type>();

            foreach (var fileInfo in pluginDir.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                if (string.Compare(fileInfo.FullName, thisAssembly.Location, true) == 0)
                    continue;
                if (string.Compare(fileInfo.FullName, entryAssembly.Location, true) == 0)
                    continue;
                try
                {
                    var assembly = Assembly.LoadFrom(fileInfo.FullName);
                    foreach (var type in assembly.GetTypes())
                    {
                        var found = false;
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            if (interfaceType == typeof(IMessagingService))
                            {
                                result.Add(type);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                            break;
                    }
                }
                catch (Exception)
                { }
            }

            return result;
        }
    }
}