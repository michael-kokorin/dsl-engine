namespace Infrastructure.Tests.Mail
{
	using System.Net;
	using System.Net.Mail;

	using NUnit.Framework;

	using Infrastructure.Mail;

	[TestFixture]
	[Ignore("Mailbox integration test")]
	public sealed class SmtpMailSenderTest
	{
		private IMailSender _target;

		[SetUp]
		public void SetUp()
		{
			var client = new SmtpClient("mail.ptsecurity.com", 25)
			{
				Credentials = new NetworkCredential("Sdl_notification", "5pK.xT#XxLha=**/"),
				EnableSsl = true
			};

			_target = new SmtpMailSender(client, "Sdl_notification@ptsecurity.com");
		}

		[Test]
		public void SendMailOverSnmp() => _target.Send(new Email
		{
			Body = "SDL Notification test.\n\n" +
				   "Please, don't reply to this message.\n\n" +
				   "---\n" +
				   "Kind regards,\n" +
				   "SDL Project Team.",
			Subject = "SDL Notification",
			To = new[]
			{
				"msharonov@ptsecurity.com"
				//,"vboronin@ptsecurity.com"
				//,"mkokorin@ptsecurity.com"
			}
		});
	}
}