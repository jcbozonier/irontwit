using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwitterPlugIn.DataObjects;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.Specs.Contacts
{
    [TestFixture]
    public class When_receiving_a_message_with_an_unknown_contact : contacts_context
    {
        [Test]
        public void It_should_have_the_name_be_the_same_as_the_username()
        {
            ViewModel.Messages.First().Contact.Name.ShouldEqual("darkxanthos");
        }

        protected override void Because()
        {
            ViewModel.ReceiveMessage.Execute(null);
        }

        protected override void Context()
        {
            TheContactProvider.Clear();
        }
    }

    [TestFixture]
    public class When_receiving_a_message_with_a_known_contact : contacts_context
    {
        [Test]
        public void Its_name_should_appear_in_message_list()
        {
            ViewModel.Messages.First().Contact.Name.ShouldEqual("Justin Bozonier");
        }

        protected override void Because()
        {
            ViewModel.ReceiveMessage.Execute(null);
        }

        protected override void Context()
        {
            
        }
    }

    public abstract class contacts_context
    {
        protected MainView ViewModel;
        protected FakeMessagingService ServiceManager;
        protected ContactProvider TheContactProvider;

        [TestFixtureSetUp]
        public void Setup()
        {
            var serviceInfo = new ServiceInformation()
            {
                ServiceID = Guid.NewGuid(),
                ServiceName = "test service"
            };

            ServiceManager = new FakeMessagingService(serviceInfo);
            TheContactProvider = new ContactProvider();

            TheContactProvider.Add(new Contact()
                                       {
                                           Name = "Justin Bozonier",
                                           ContactId = Guid.NewGuid(),
                                           Identities = new[]{new Identity("darkxanthos", serviceInfo)}
                                       });

            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<MainView>().TheDefaultIsConcreteType<MainView>();
                x.ForRequestedType<IInteractionContext>().TheDefaultIsConcreteType<TestingInterface>();
                x.ForRequestedType<IMessagingServiceManager>().TheDefault.IsThis(ServiceManager);
                x.ForRequestedType<IContactProvider>().TheDefault.IsThis(TheContactProvider);
            });

            ViewModel = ObjectFactory.GetInstance<MainView>();

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }

    public class TestingInterface : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            return new Credentials()
                       {
                           Password = "password",
                           UserName = "username",
                           ServiceInformation = serviceInformation
                       };
        }

        public bool AuthenticationFailedRetryQuery()
        {
            throw new System.NotImplementedException();
        }
    }

    public class FakeMessage : IMessage
    {
        public string Text { get; set; }
        public IIdentity Address { get; set; }
    }

    public class FakeMessagingService : IMessagingServiceManager
    {
        public ServiceInformation ServiceInfo;
        private ServiceInformation _Information;

        public FakeMessagingService(ServiceInformation info)
        {
            _Information = info;
        }

        public bool CanAccept(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>()
                       {
                           new FakeMessage()
                               {
                                   Text = "message text",
                                   Address = new Identity("darkxanthos", _Information)
                               }
                       };
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            throw new System.NotImplementedException();
        }

        public void SetCredentials(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public bool CanFind(string address)
        {
            throw new System.NotImplementedException();
        }

        public ServiceInformation GetInformation()
        {
            throw new System.NotImplementedException();
        }

        public void StartReceiving()
        {
            throw new System.NotImplementedException();
        }

        public void StopReceiving()
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public void SendMessage(string recipient, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
