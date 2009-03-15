namespace Unite.Messaging.Entities
{
    public interface IMessage
    {
        string Text { get; set; }
        IIdentity Address { get; set; }
    }
}