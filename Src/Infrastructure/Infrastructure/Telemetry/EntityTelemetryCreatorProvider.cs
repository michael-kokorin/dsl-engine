namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	internal sealed class EntityTelemetryCreatorProvider : IEntityTelemetryCreatorProvider
	{
		private readonly IUnityContainer _unityContainer;

		public EntityTelemetryCreatorProvider([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IEntityTelemetryCreator<T> Resolve<T>() where T : class =>
			_unityContainer.Resolve<IEntityTelemetryCreator<T>>();
	}
}