using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Unite.Messaging
{
    public interface IServiceProvider
    {
        void Add(params IMessagingService[] services);
        IEnumerable<IMessagingService> GetServices();
        event EventHandler<CredentialEventArgs> CredentialsRequested;
    }

    public class ServiceProvider : IServiceProvider
    {
        private readonly List<IMessagingService> Services;

        public ServiceProvider()
        {
            Services = new List<IMessagingService>();

            // We should probably try to discover the plug ins here...?

            var mainExeDir = Environment.CurrentDirectory;
            var pluginDir = new DirectoryInfo(mainExeDir);
            foreach (var fileInfo in pluginDir.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                var thisAssembly = Assembly.GetExecutingAssembly();
                var entryAssembly = Assembly.GetEntryAssembly();
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
                                AddServiceProvider(type);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                            break;
                    }
                }
                catch
                { }
            }
        }

        private void AddServiceProvider(Type serviceType)
        {
            var constructor = serviceType.GetConstructor(new Type[0]);
            var serviceObject = constructor.Invoke(new object[0]);
            var service = (IMessagingService) serviceObject;
            Add(service);
        }

        public void Add(params IMessagingService[] services)
        {
            foreach(var service in services)
            {
                Add(service);
            }
        }

        private void Add(IMessagingService service)
        {
            service.CredentialsRequested += _GetCredentials;
            Services.Add(service);
        }

        private void _GetCredentials(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(this, e);
        }

        public IEnumerable<IMessagingService> GetServices()
        {
            return Services;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
    }
}
