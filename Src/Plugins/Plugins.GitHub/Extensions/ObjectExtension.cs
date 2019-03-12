namespace Plugins.GitHub.Extensions
{
	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;

	internal static class ObjectExtension
	{
		[NotNull]
		public static User ToModel([NotNull] this Octokit.User user) =>
			new User
			{
				DisplayName = user.Name,
				Login = user.Login,
				InfoUrl = user.HtmlUrl
			};
	}
}