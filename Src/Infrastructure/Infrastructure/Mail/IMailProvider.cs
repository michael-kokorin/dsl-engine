namespace Infrastructure.Mail
{
    public interface IMailProvider
    {
        IMailSender BeginSend();
    }
}