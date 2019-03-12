namespace Modules.UI.Extensions
{
	using Modules.UI.Services;

	public static class AuthorityProviderExtension
	{
		public static bool IsCan(this IAuthorityProvider authorityProvider, string authorityName, long? entityId) =>
			authorityProvider.IsCan(new[] {authorityName}, entityId);
	}
}