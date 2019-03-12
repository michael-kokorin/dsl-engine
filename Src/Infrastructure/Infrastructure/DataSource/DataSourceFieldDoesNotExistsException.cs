namespace Infrastructure.DataSource
{
	using System;

	using Common.Extensions;
	using Infrastructure.Resources;

	internal sealed class DataSourceFieldDoesNotExistsException : Exception
	{
		public DataSourceFieldDoesNotExistsException(string dataSourceName, string fieldName)
			: base(Resources.DataSourceFieldDoesNotExistsExceptionMessageByName.FormatWith(fieldName, dataSourceName))
		{
		}
	}
}