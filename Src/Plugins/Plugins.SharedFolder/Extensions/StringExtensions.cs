namespace Plugins.SharedFolder.Extensions
{
	using JetBrains.Annotations;

	internal static class StringExtensions
	{
		[NotNull]
		public static string AsRelative([NotNull] this string relativepath) => relativepath.TrimStart('\\', '/');
	}
}