namespace Infrastructure.Tags
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Repository;

	[UsedImplicitly]
	internal sealed class TagEntityRepositoryProvider : ITagEntityRepositoryProvider
	{
		private readonly IUnityContainer _unityContainer;

		public TagEntityRepositoryProvider([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IReadRepository<T> Get<T>() where T : class, IEntity => _unityContainer.Resolve<IReadRepository<T>>();
	}
}