namespace Infrastructure
{
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Configuration service parameters
	/// https://wiki.ptsecurity.com/display/AISSDL/Configuration+parameters
	/// </summary>
	[SuppressMessage("ReSharper", "MemberCanBeInternal")]
	public static class ConfigurationKeys
	{
		/// <summary>
		/// Application configuration settings
		/// </summary>
		public static class AppSettings
		{
			public const string ActiveDirectoryRootGroup = "ActiveDirectoryRootGroup";

			public const string TempDirPath = "TempDirPath";

			public const string HostName = "HostName";

			public const string ReportLogDir = "ReportLogDir";
		}

		/// <summary>
		/// Mail sender parameters
		/// </summary>
		public static class MailOutbox
		{
			public const string MailBox = "MailOutbox";

			public const string Host = "MailOutboxHost";

			public const string Password = "MailOutboxPassword";

			public const string Port = "MailOutboxPort";

			public const string SslEnabled = "MailOutboxSslEnabled";

			public const string User = "MailOutboxUser";
		}
	}
}