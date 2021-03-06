﻿using System;
using System.Collections.Generic;
using StructureMap;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly List<IMessagingService> Services;

        public ServiceProvider(IPluginFinder finder)
        {
            Services = new List<IMessagingService>();

            var plugins = finder.GetAllPlugins();

            foreach(var plugin in plugins)
            {
                _AddServiceProvider(plugin);
            }
        }

        private void _AddServiceProvider(Type serviceType)
        {
            var service = (IMessagingService)ObjectFactory.GetInstance(serviceType);
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
            service.AuthorizationFailed += service_AuthorizationFailed;
            Services.Add(service);
        }

        void service_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            if (AuthorizationFailed != null)
                AuthorizationFailed(sender, e);
        }

        private void _GetCredentials(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(sender, e);
        }

        public IEnumerable<IMessagingService> GetServices()
        {
            return Services;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

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