using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public interface IServiceResolver
    {
        ServiceInformation GetService(string address);
    }
}
