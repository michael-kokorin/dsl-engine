namespace Infrastructure.Mail
{
    public interface IMailClient
    {
        IMailSender BeginSend(MailConnectionParameters parameters);
    }
}