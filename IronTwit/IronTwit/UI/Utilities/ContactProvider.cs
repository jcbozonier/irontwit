using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;

namespace Unite.UI.Utilities
{
    public interface IContactProvider
    {
        Contact Get(IIdentity identity);
        void Add(Contact contact);
    }

    public class ContactProvider : IContactProvider
    {
        private List<Contact> Contacts;

        public ContactProvider()
        {
            Contacts = new List<Contact>();
        }

        public Contact Get(IIdentity identity)
        {
            foreach(var contact in Contacts)
            {
                foreach(var contactIdentity in contact.Identities)
                {
                    if(contactIdentity.Equals(identity)) return contact;
                }
            }

            return new Contact()
                       {
                           Identities = new[] {identity}, 
                           Name = identity.UserName
                       };
        }

        public void Add(Contact contact)
        {
            Contacts.Add(contact);
        }

        public void Clear()
        {
            Contacts.Clear();
        }
    }
}
