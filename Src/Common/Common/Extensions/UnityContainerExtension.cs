namespace Common.Extensions
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.ObjectBuilder2;
	using Microsoft.Practices.Unity;

	using Common.Command;
	using Common.Container;
	using Common.Query;

	/// <summary>
	///   Provides extension methods to work with IoC-container.
	/// </summary>
	public static class UnityContainerExtension
	{
		private static LifetimeManager GetLifetimeManager(ReuseScope reuseScope) => new LifeManagerFactory().Build(reuseScope);

		public static IUnityContainer RegisterType<T>(this IUnityContainer container, ReuseScope reuseScope)
			=> container.RegisterType<T>(GetLifetimeManager(reuseScope));

		public static IUnityContainer RegisterType<TA>(
			this IUnityContainer container,
			Type to,
			ReuseScope reuseScope) =>
				container.RegisterType(typeof(TA), to, GetLifetimeManager(reuseScope));

		public static IUnityContainer RegisterType<TA>(
			this IUnityContainer container,
			Type to,
			string name,
			ReuseScope reuseScope) =>
				container.RegisterType(typeof(TA), to, name, GetLifetimeManager(reuseScope));

		public static IUnityContainer Register(
			this IUnityContainer container,
			IContainerModule module,
			ReuseScope reuseScope)
			=> module.Register(container, reuseScope);

		public static IUnityContainer Register(
			this IUnityContainer container,
			IEnumerable<IContainerModule> modules,
			ReuseScope reuseScope)
		{
			modules.ForEach(_ => _.Register(container, reuseScope));

			return container;
		}

		/// <summary>
		///   Registers the command handler.
		/// </summary>
		/// <typeparam name="TCommand">The type of the command.</typeparam>
		/// <typeparam name="THandler">The type of the handler.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterCommandHandler<TCommand, THandler>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TCommand : ICommand
			where THandler : ICommandHandler<TCommand> => container
				.RegisterType<ICommandHandler<TCommand>, THandler>(reuseScope);

		/// <summary>
		///   Registers the data query handler.
		/// </summary>
		/// <typeparam name="TQuery">The type of the query.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <typeparam name="THandler">The type of the handler.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container</returns>
		[UsedImplicitly]
		public static IUnityContainer RegisterDataQueryHandler<TQuery, TResult, THandler>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TQuery : class, IDataQuery
			where THandler : IDataQueryHandler<TQuery, TResult> => container
				.RegisterType<IDataQueryHandler<TQuery, TResult>, THandler>();

		/// <summary>
		///   Registers the type.
		/// </summary>
		/// <typeparam name="TContract">The type of the contract.</typeparam>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterType<TContract, TInstance>(this IUnityContainer container, ReuseScope reuseScope)
			where TInstance : TContract => container.RegisterType<TContract, TInstance>(GetLifetimeManager(reuseScope));

		/// <summary>
		///   Registers the type.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="contractType">Type of the contract.</param>
		/// <param name="instanceType">Type of the instance.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		[UsedImplicitly]
		public static IUnityContainer RegisterType(
			this IUnityContainer container,
			Type contractType,
			Type instanceType,
			ReuseScope reuseScope) =>
				container.RegisterType(contractType, instanceType, GetLifetimeManager(reuseScope));

		/// <summary>
		///   Registers the type with named mapping.
		/// </summary>
		/// <typeparam name="TContract">The type of the contract.</typeparam>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="mappingName">Name of the mapping.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>THe container.</returns>
		public static IUnityContainer RegisterType<TContract, TInstance>(
			this IUnityContainer container,
			string mappingName,
			ReuseScope reuseScope)
			where TInstance : TContract
			=> container.RegisterType<TContract, TInstance>(mappingName, GetLifetimeManager(reuseScope));

		/// <summary>
		///   Registers the type with injection members.
		/// </summary>
		/// <typeparam name="TContract">The type of the contract.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <param name="injectionMembers">The injection members.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterType<TContract>(
			this IUnityContainer container,
			ReuseScope reuseScope,
			params InjectionMember[] injectionMembers) =>
				container.RegisterType<TContract>(GetLifetimeManager(reuseScope), injectionMembers);

		public static IUnityContainer RegisterInstance<TAs>(this IUnityContainer container,
			object instance,
			ReuseScope reuseScope) => container.RegisterInstance(typeof(TAs), instance, GetLifetimeManager(reuseScope));
	}
}