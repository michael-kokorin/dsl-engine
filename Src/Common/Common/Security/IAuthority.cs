namespace Common.Security
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///   Represents contract for authorities.
	/// </summary>
	internal interface IAuthority
	{
		/// <summary>
		///   Gets all authorities.
		/// </summary>
		/// <returns>Collection of authorities.</returns>
		[UsedImplicitly]
		IEnumerable<string> All();
	}
}