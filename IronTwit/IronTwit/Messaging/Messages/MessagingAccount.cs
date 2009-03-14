namespace Unite.Messaging.Messages
{
    public struct MessagingAccount
    {
        public MessagingAccount(IMessagingService service, Credentials credentials)
        {
            Service = service;
            Credentials = credentials;
        }

        public IMessagingService Service;
        public Credentials Credentials;
    }
}