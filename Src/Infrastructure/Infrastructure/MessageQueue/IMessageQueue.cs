namespace Infrastructure.MessageQueue
{
    public interface IMessageQueue
    {
        IQueueReader BeginRead(MessageQueueKeys key);

        IQueueWriter BeginWrite(MessageQueueKeys key);
    }
}