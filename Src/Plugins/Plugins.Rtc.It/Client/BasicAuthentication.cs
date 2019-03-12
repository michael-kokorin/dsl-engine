namespace Plugins.Rtc.It.Client
{
	using System;

	internal sealed class BasicAuthentication
	{
		public readonly string Username;

		public readonly string Password;

		public BasicAuthentication(string username, string password)
		{
			if (username == null) throw new ArgumentNullException(nameof(username));
			if (password == null) throw new ArgumentNullException(nameof(password));

			Username = username;
			Password = password;
		}
	}
}