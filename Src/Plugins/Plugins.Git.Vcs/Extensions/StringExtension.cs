namespace Plugins.Git.Vcs.Extensions
{
	internal static class StringExtension
	{
		public static string FromCanonical(this string canonicalName) =>
			canonicalName?.Replace("refs/heads/", "");
	}
}