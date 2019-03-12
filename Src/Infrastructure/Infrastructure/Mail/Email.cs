namespace Infrastructure.Mail
{
	using System.Collections.Generic;
	using System.IO;

	public sealed class Email
	{
		public IEnumerable<Attachment> Attachments { get; set; }

		public string Body { get; set; }

		public bool IsHtml { get; set; }

		public string Subject { get; set; }

		public IEnumerable<string> To { get; set; }
	}

	public sealed class Attachment
	{
		public string Title { get; set; }

		public MemoryStream Content { get; set; }
	}
}