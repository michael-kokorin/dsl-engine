namespace Common.Security
{
	using System;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides methods to get user SID>
	/// </summary>
	public interface IUserPrincipalProvider
	{
		/// <summary>
		///   Gets the current user SID.
		/// </summary>
		/// <returns>The current user SID.</returns>
		UserPrincipalInfo Get();

		/// <summary>
		///   Determines whether the current user is in role with the specified SID.
		/// </summary>
		/// <param name="roleSid">The role SID.</param>
		/// <returns><see langword="true"/> if the current user is in role; otherwise, <see langword="false"/>.</returns>
		bool IsCurrentUserInRole(string roleSid);
	}

	public sealed class UserPrincipalInfo
	{
		public readonly string Domain;

		public readonly string Sid;

		public readonly string Name;

		public UserPrincipalInfo([NotNull] string sid, [NotNull] string name, [NotNull] string domain)
		{
			if (sid == null) throw new ArgumentNullException(nameof(sid));
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (domain == null) throw new ArgumentNullException(nameof(domain));

			Sid = sid;
			Name = name;
			Domain = domain;
		}
	}
}