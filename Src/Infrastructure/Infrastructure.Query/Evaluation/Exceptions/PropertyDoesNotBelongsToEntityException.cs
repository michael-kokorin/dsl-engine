namespace Infrastructure.Query.Evaluation.Exceptions
{
	using System;

	using Common.Extensions;
	using Infrastructure.Query.Resources;

	internal sealed class PropertyDoesNotBelongsToEntityException : Exception
	{
		public PropertyDoesNotBelongsToEntityException(string entityName, string propertyName)
			: base(Resources.PropertyDoesNotBelongsToEntityExceptionMessage.FormatWith(propertyName, entityName))
		{
		}
	}
}