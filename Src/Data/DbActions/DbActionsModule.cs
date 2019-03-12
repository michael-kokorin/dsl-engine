namespace DbActions
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using DbActions.ActionsStore;

	/// <summary>
	///   Represents container module for this project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	[UsedImplicitly]
	public sealed class DbActionsModule: IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IDbActionProvider, DbActionProvider>(reuseScope)
				.RegisterType<IDbAction, AddRoleAction>(typeof(AddRoleAction).Name, reuseScope)
				.RegisterType<IDbAction, AddProjectAction>(typeof(AddProjectAction).Name, reuseScope);
	}
}