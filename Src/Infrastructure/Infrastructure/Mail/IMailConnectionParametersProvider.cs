namespace Infrastructure.Mail
{
    public interface IMailConnectionParametersProvider
    {
        MailConnectionParameters Get();

        MailConnectionParameters TryGet();

        void Set(MailConnectionParameters parameters);
    }
}