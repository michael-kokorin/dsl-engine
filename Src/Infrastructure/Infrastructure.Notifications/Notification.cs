namespace Infrastructure.Notifications
{
	using Common.Enums;

	public sealed class Notification
	{
		public NotificationAttachment[] Attachments { get; set; }

		public string Message { get; set; }

		public NotificationProtocolType Protocol { get; set; }

		public string[] Targets { get; set; }

		public string Title { get; set; }
	}

	public sealed class NotificationAttachment
	{
		public string Title { get; set; }

		public byte[] Content { get; set; }
	}
}