namespace Infrastructure.MessageQueue
{
    using System;

    public interface IQueueWriter : IDisposable
    {
        void Send(string message);
    }
}