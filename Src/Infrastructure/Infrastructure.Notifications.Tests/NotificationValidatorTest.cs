namespace Infrastructure.Notifications.Tests
{
	using System;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Validation;
	using Infrastructure.Notifications;

	[TestFixture]
	public sealed class NotificationValidatorTest
	{
		private IValidator<Notification> _target;

		[SetUp]
		public void SetUp() => _target = new NotificationValidator();

		[Test]
		public void ShouldNotValidateNullNotification() => Assert.Throws<ArgumentNullException>(() => _target.Validate(null));

		[Test]
		public void ShouldNotValidateNullMessage()
		{
			var notification = new Notification
			{
				Message = null,
				Protocol = NotificationProtocolType.Email,
				Targets = new[] {"mail"},
				Title = "title"
			};

			Assert.Throws<ArgumentNullException>(() => _target.Validate(notification));
		}

		[Test]
		public void ShouldNotValidateNullTargets()
		{
			var notification = new Notification
			{
				Message = "Message",
				Protocol = NotificationProtocolType.Email,
				Targets = null,
				Title = "title"
			};

			Assert.Throws<ArgumentNullException>(() => _target.Validate(notification));
		}

		[Test]
		public void ShouldNotValidateEmptyTargets()
		{
			var notification = new Notification
			{
				Message = "Message",
				Protocol = NotificationProtocolType.Email,
				Targets = new string[0],
				Title = "title"
			};

			Assert.Throws<ArgumentException>(() => _target.Validate(notification));
		}

		[Test]
		public void ShouldValidateNotification()
		{
			var notification = new Notification
			{
				Message = "message",
				Protocol = NotificationProtocolType.Email,
				Targets = new[] {"mail"},
				Title = "title"
			};

			_target.Validate(notification);
		}
	}
}