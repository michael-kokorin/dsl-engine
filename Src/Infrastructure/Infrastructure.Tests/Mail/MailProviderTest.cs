namespace Infrastructure.Tests.Mail
{
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Infrastructure.Mail;

    [TestFixture]
    public sealed class MailProviderTest
    {
        private IMailProvider _target;

        private Mock<IMailClient> _mailClient;

        private Mock<IMailConnectionParametersProvider> _mailConnectionParametersProvider;

        [SetUp]
        public void SetUp()
        {
            _mailClient = new Mock<IMailClient>();
            _mailConnectionParametersProvider = new Mock<IMailConnectionParametersProvider>();

            _target = new MailProvider(_mailClient.Object, _mailConnectionParametersProvider.Object);
        }

        [Test]
        public void ShouldReturnMailSender()
        {
            var parameters = new MailConnectionParameters();

            _mailConnectionParametersProvider
                .Setup(_ => _.Get())
                .Returns(parameters);

            var mailSender = Mock.Of<IMailSender>();

            _mailClient
                .Setup(_ => _.BeginSend(parameters))
                .Returns(mailSender);

            var result = _target.BeginSend();

            result.Should().NotBeNull();
            result.ShouldBeEquivalentTo(mailSender);
        }

        [Test]
        public void ShouldThrowMailParamenersException() => Assert.Throws<IncorrectMailSettingsException>(() => _target.BeginSend());

        [Test]
        [Ignore("Integration test")]
        public void ShouldSendMail()
        {
            var provider = new MailProvider(new MailClient(), _mailConnectionParametersProvider.Object);

            _mailConnectionParametersProvider
                .Setup(_ => _.Get())
                .Returns(new MailConnectionParameters
                {
                    Host = "mail.ptsecurity.com",
                    IsSslEnabled = true,
                    Mailbox = "Sdl_notification@ptsecurity.com",
                    Password = "5pK.xT#XxLha=**/",
                    Port = 25,
                    Username = "Sdl_notification"
                });

            using (var sender = provider.BeginSend())
            {
                sender.Send(new Email
                {
                    Body = "Hello, body",
                    Subject = "Halo, subject",
                    To = new[] {"msharonov@ptsecurity.com"}
                });
            }
        }
    }
}