namespace Infrastructure.Engines
{
	using System;

	using JetBrains.Annotations;

	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class QueryEntityNameTranslator : IQueryEntityNameTranslator
	{
		private readonly IQueryEntityNameRepository _entityNameRepository;

		public QueryEntityNameTranslator([NotNull] IQueryEntityNameRepository entityNameRepository)
		{
			if (entityNameRepository == null) throw new ArgumentNullException(nameof(entityNameRepository));

			_entityNameRepository = entityNameRepository;
		}

		public Type GetEntityType(string entityName)
		{
			if (string.IsNullOrEmpty(nameof(entityName)))
				throw new ArgumentNullException(nameof(entityName));

			var entityTypeInfo = _entityNameRepository.GetByKey(entityName);

			if (entityTypeInfo == null)
				throw new UnknownQueryEntityTypeException(entityName);

			var typeFullName = $"{entityTypeInfo.TypeName}, {entityTypeInfo.AssemblyName}";

			var entityType = Type.GetType(typeFullName);

			return entityType;
		}
	}
}