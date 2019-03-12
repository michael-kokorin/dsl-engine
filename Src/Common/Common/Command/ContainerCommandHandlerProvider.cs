namespace Common.Command
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class ContainerCommandHandlerProvider: ICommandHandlerProvider
	{
		private readonly IUnityContainer _container;

		public ContainerCommandHandlerProvider([NotNull] IUnityContainer container)
		{
			if(container == null) throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		/// <summary>
		///   Resolves command handler of specific type.
		/// </summary>
		/// <typeparam name="T">Type of command to handle.</typeparam>
		/// <returns>Command handler.</returns>
		public ICommandHandler<T> Resolve<T>() where T: class, ICommand => _container.Resolve<ICommandHandler<T>>();
	}
}