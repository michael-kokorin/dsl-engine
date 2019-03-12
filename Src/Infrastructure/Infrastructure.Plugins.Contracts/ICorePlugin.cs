namespace Infrastructure.Plugins.Contracts
{
	using System.ComponentModel.Composition;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;

	/// <summary>
	///     Plugin base interface
	/// </summary>
	[InheritedExport(typeof(ICorePlugin))]
	public interface ICorePlugin: IPlugin
	{
		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		[NotNull]
		User GetCurrentUser();
	}
}