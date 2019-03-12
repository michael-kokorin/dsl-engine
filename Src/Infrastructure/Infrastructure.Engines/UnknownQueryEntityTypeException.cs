namespace Infrastructure.Engines
{
	using System;

	using Common.Extensions;

	internal sealed class UnknownQueryEntityTypeException : Exception
	{
		public UnknownQueryEntityTypeException(string entityTypeName)
			: base(Resources.Resources.UnknownQueryEntityTypeExceptionMessage.FormatWith(entityTypeName))
		{
		}
	}
}