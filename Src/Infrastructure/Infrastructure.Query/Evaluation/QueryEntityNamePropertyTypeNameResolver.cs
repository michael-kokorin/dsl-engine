namespace Infrastructure.Query.Evaluation
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines;
	using Infrastructure.Query.Evaluation.Exceptions;

	[UsedImplicitly]
	internal sealed class QueryEntityNamePropertyTypeNameResolver : IQueryEntityNamePropertyTypeNameResolver
	{
		private readonly IQueryEntityNameTranslator _queryEntityNameTranslator;

		public QueryEntityNamePropertyTypeNameResolver([NotNull] IQueryEntityNameTranslator queryEntityNameTranslator)
		{
			if (queryEntityNameTranslator == null) throw new ArgumentNullException(nameof(queryEntityNameTranslator));

			_queryEntityNameTranslator = queryEntityNameTranslator;
		}

		public string ResolvePropertyTypeName(string entityTypeName, string propertyName)
		{
			var entityType = _queryEntityNameTranslator.GetEntityType(entityTypeName);

			var property = entityType.GetProperty(propertyName);

			if (property == null)
				throw new PropertyDoesNotBelongsToEntityException(entityTypeName, propertyName);

			if (property.PropertyType.IsGenericType)
			{
				return property.PropertyType.GenericTypeArguments[0].Name;
			}

			var propertyTypeName = property.PropertyType.Name;

			return propertyTypeName;
		}
	}
}