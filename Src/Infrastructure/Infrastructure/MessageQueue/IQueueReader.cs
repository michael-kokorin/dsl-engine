namespace Infrastructure.MessageQueue
{
    using System;

    public interface IQueueReader : IDisposable
    {
        string Read();
    }
}