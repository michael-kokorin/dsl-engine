namespace Infrastructure.Security
{
	using System.Collections.Generic;

	public interface IAuthorityProvider
	{
		Authority Create(string authorityKey, string displayName);

		Authority Get(string authorityKey);

		IEnumerable<Authority> Get(long roleId, long? projectId = null);

		void Grant(long roleId, IEnumerable<string> authorityKeys);
	}
}