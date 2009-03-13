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
        IMessagingService GetService(ServiceInformation info);
    }

    public class ServiceProvider : IServiceProvider
    {
        private readonly List<IMessagingService> Services;

        public ServiceProvider()
        {
            Services = new List<IMessagingService>();

            // We should probably try to discover the plug ins here...?
            _LoadServicePlugins();
        }

        private void _LoadServicePlugins()
        {
            var mainExeDir = Environment.CurrentDirectory;
            var pluginDir = new DirectoryInfo(mainExeDir);
            var thisAssembly = Assembly.GetExecutingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();

            if(entryAssembly == null) return;

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
        public IMessagingService GetService(ServiceInformation info)
        {
            if(Services == null)
                throw new NullReferenceException("This service provider has a null collection of services. You should NEVER have this issue.");

            foreach (var service in Services)
            {
                if(service.GetInformation().Equals(info)) return service;
            }

            throw new InvalidOperationException("The requested service does not exist. You should NEVER see this.");
        }
    }
}
