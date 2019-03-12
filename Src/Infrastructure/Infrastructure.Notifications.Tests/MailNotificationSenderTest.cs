namespace Infrastructure.Notifications.Tests
{
	using System.Collections.Generic;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Logging;
	using Infrastructure.Mail;
	using Infrastructure.Notifications;

	[TestFixture]
	public sealed class MailNotificationSenderTest
	{
		private INotificationSender _target;

		private Mock<ILog> _log;

		private Mock<IMailProvider> _mailProvider;

		private Mock<IMailSender> _mailSender;

		[SetUp]
		public void SetUp()
		{
			_mailProvider = new Mock<IMailProvider>();

			_mailSender = new Mock<IMailSender>();

			_mailProvider.Setup(_ => _.BeginSend()).Returns(_mailSender.Object);

			_log = new Mock<ILog>();

			_target = new MailNotificationSender(_log.Object, _mailProvider.Object);
		}

		[Test]
		public void ShouldCreateAndSendMailOverSender()
		{
			var recipients = new[]
			{
				"mail1",
				"mail2"
			};

			const string title = "mail title";

			const string message = "mail message";

			_target.Send(new Notification
			{
				Targets = recipients,
				Message = message,
				Title = title
			});

			_mailProvider.Verify(_ => _.BeginSend(), Times.Once);

			_mailSender.Verify(_ => _.Send(It.Is<Email>(m =>
				m.Body.Equals(message) &&
				m.Subject.Equals(title) &&
				MathArrays(m.To, recipients))));
		}

		private static bool MathArrays<T>(IEnumerable<T> source, IEnumerable<T> target)
		{
			source.ShouldAllBeEquivalentTo(target);

			return true;
		}
	}
}