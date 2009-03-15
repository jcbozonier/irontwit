using Unite.Messaging.Entities;

namespace Unite.Messaging
{
    public interface IInteractionContext
    {
        Credentials GetCredentials(IServiceInformation serviceInformation);
        bool AuthenticationFailedRetryQuery();
    }
}
