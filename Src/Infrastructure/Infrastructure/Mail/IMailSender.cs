namespace Infrastructure.Mail
{
    using System;

    public interface IMailSender : IDisposable
    {
        void Send(Email mail);
    }
}